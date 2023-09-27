using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Inserter : NetworkBehaviour
{
    [SerializeField] private Item item;
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
            Item instItem = Instantiate(item);
            instItem.GetComponent<NetworkObject>().Spawn();
            connectedBelt.externalAddItemToBelt(instItem, 0);
            
            time = 0;
        }
    }
}
