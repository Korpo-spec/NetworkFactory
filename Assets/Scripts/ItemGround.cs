using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class ItemGround : NetworkBehaviour
{
    [SerializeField]public Item item 
    {
        get => intItem;
        set
        {
            SetItem(value.itemID);
        }
        
    }

    private Item intItem;
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += SyncItems;

    }

    private bool joinThing;
    public override void OnNetworkSpawn()
    {
        Debug.Log("Running");
        if (!joinThing)
        {
            return;
        }
        Collider2D[] col = Physics2D.OverlapPointAll(transform.position);

        foreach (var collidingObj in col)
        {
            if(!collidingObj.CompareTag("Belt")) continue;
            Belt belt = collidingObj.GetComponent<Belt>();
            float offset = ((belt.transform.position + belt.direction / 2) - transform.position).magnitude /
                           belt.transform.localScale.x;
            Debug.Log(offset, belt);
            if (offset > 1 || offset < 0)
            {
                Debug.LogError("offset is to big or small: " + offset);
            }

            
            belt.externalAddItemToBelt(this, 1 - offset);
        }  
        
        
        
    }

    private void SyncItems(ulong playerId)
    {
        Debug.Log("Sync");
        joinThing = true;
    }

    private void SetItem(FixedString32Bytes itemID)
    {
        SetItemClientRpc(itemID);
    }

    [ClientRpc]
    private void SetItemClientRpc(FixedString32Bytes itemID)
    {
        //Debug.Log("Sent sprite Data");
        var itemForSprite = Instantiate(ItemDictionary.Items[itemID]);
        SetSprite(itemForSprite);
        intItem = itemForSprite;
        intItem.itemVisualizer = this;
    }

    private void SetSprite(Item item)
    {
        GetComponent<SpriteRenderer>().sprite = item.icon;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    
    
}
