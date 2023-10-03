using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.Netcode;
using UnityEngine;

public class Furnace : Building
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
    
}
