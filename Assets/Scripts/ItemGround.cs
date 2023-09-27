using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ItemGround : NetworkBehaviour
{
    [SerializeField]private Item item;
    
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

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}
