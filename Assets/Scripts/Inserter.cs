using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Inserter : NetworkBehaviour
{
    [SerializeField] private ItemGround item;
    [SerializeField] private Belt connectedBelt;
    [SerializeField] private bool on;
    private NetworkObject networkObj;
    private float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        networkObj = GetComponent<NetworkObject>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 1f && on)
        {
            AddToBeltServerRpc();
            
            time = 0;
        }
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
}
