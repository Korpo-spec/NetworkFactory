using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.Netcode;
using UnityEngine;

public class Inserter : Building
{
    [SerializeField] private ItemGround item;
    [SerializeField] private Belt connectedBelt;
    [SerializeField] private bool on;
    public override NetworkObject NetworkObject => GetComponent<NetworkObject>();
    private NetworkObject networkObj;
    

    private BeltItemData currentItem;
    // Start is called before the first frame update
    void Start()
    {
        networkObj = GetComponent<NetworkObject>();
    }

    // Update is called once per frame
    void Update()
    {
        

        if ( IsHost &&!item&& input != null )
        {
            
            int amount = 1;
            
            Item itemObj = input.GetItem(ref amount);
            if (!itemObj)
            {
                return;
            }
            Debug.Log("Instatiate");
            item = Instantiate(IInserterInteract.itemGround, transform.position - transform.right/2, Quaternion.identity);
            item.item = itemObj;
            item.GetComponent<NetworkObject>().Spawn();
            
            SendInserterDataClientRpc(item.GetComponent<NetworkObject>().NetworkObjectId, TickClock.instance.clock + 1.0f);
            
        }
        

        if(!item) return;
        item.transform.position = Vector3.Lerp(transform.position + transform.right / 2,transform.position - transform.right / 2, TickClock.instance.TimeLeft(currentItem.pos, 1));
        if (TickClock.instance.TimeLeft(currentItem.pos,1 ) > 1.0f)
        {
            currentItem = default;
            output.AddItem(item, 1);
            item = null;
        }
    }

    [ClientRpc]
    private void SendInserterDataClientRpc(ulong networkID, float timePos)
    {
        item = GetNetworkObject(networkID).GetComponent<ItemGround>();
        currentItem = new BeltItemData(item, timePos);
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddToBeltServerRpc()
    {
        ItemGround instItem = Instantiate(item);
        instItem.GetComponent<NetworkObject>().Spawn();
        AddToBeltClientRpc(instItem.GetComponent<NetworkObject>().NetworkObjectId);
        
    }

    [ClientRpc]
    private void AddToBeltClientRpc(ulong objectID)
    {
        NetworkObject obj = GetNetworkObject(objectID);
        connectedBelt.externalAddItemToBelt(obj.GetComponent<ItemGround>(), 0);
    }

    private void RemoveFromBelt()
    {
        
    }

    
    private IInserterInteract input;
    private IInserterInteract output;

    public override void OnPlace(Map map, Vector2 pos)
    {
        var g = map.map.GetValue(pos + (Vector2) transform.right);
        
        var g2 = map.map.GetValue(pos + (Vector2) transform.right * -1);

        var interact = g as IInserterInteract;
        if (interact != null)
        {
            Debug.Log(interact);
            input = interact;
        }
        
        interact = g2 as IInserterInteract;
        if (interact != null)
        {
            Debug.Log(interact);
            output = interact;
        }
        
    }
}
