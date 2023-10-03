using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu()]
public class ItemDictionary : ScriptableObject
{

    [SerializeField] private List<Item> itemNoDictionary;

    public Dictionary<FixedString32Bytes, Item> Items = new Dictionary<FixedString32Bytes, Item>();
    [SerializeField] private bool update;

    public void AddListToDictionary()
    {
        Items = new Dictionary<FixedString32Bytes, Item>();
        foreach (var item in itemNoDictionary)
        {
            Items.Add(item.itemID, item);
        }
    }
    
}
