using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Item : ScriptableObject
{
    public string name;
    public int stackSize;
    public FixedString32Bytes itemID => "baseGame:" + name.Trim();


}
