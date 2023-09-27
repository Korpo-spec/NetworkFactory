using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TickClock : NetworkBehaviour
{
    
    public static TickClock instance { get; private set; }
    
    public float clock;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
        if (IsHost)
        {
            clock = Time.time;
        }
    }

    public override void OnNetworkSpawn()
    {
        SyncClockServerRpc(NetworkObjectId);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsHost)
        {
            clock = Time.time;
        }
        else
        {
            clock += Time.deltaTime;
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void SyncClockServerRpc(ulong syncClient)
    {
        SyncClockClientRpc(syncClient, clock);
    }
    
    [ClientRpc]
    private void SyncClockClientRpc(ulong syncClient, float clockTime)
    {
        if (NetworkObjectId == syncClient)
        {
            clock = clockTime;
        }
    }
}
