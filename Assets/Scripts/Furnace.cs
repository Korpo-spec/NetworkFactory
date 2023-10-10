using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.Netcode;
using UnityEngine;

public class Furnace : Building, IInserterInteract
{
    public override Vector2 size => Vector2.one * 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override NetworkObject NetworkObject => GetComponent<NetworkObject>();

    public Item GetItem(ref int amount)
    {
        throw new System.NotImplementedException();
    }

    public bool AddItem(ItemGround item, int amount)
    {
        throw new System.NotImplementedException();
    }
}
