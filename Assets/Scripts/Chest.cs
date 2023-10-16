using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.Netcode;
using UnityEngine;

public class Chest : Building, IInserterInteract
{
    [SerializeField] private Item itemInChest;
    [SerializeField] private int amountInChest;
    public override Vector2 size => Vector2.one;

    public override NetworkObject NetworkObject => GetComponent<NetworkObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Item GetItem(ref int amount, List<Item> neededItems)
    {
        amount = 1;
        return amountInChest-- > 0 ? Instantiate(itemInChest) : null;
    }

    public bool AddItem(ItemGround item, int amount)
    {
        throw new System.NotImplementedException();
    }
}
