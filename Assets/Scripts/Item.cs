using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu()]
public class Item : ScriptableObject
{
    public string name;
    public int stackSize;
    public Sprite icon;
    public FixedString32Bytes itemID => "baseGame:" + name.Trim().Replace(' ', '_');

    public Building building;

    [NonSerialized]public ItemGround itemVisualizer;

    private void OnValidate()
    {
        if (building)
        {
            building.buildingID = itemID;
        }
        
    }
}
