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
    void FixedUpdate()
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

    public float TimeLeft(float endtime, float time)
    {
        float timeDif = (endtime-clock) / time;
        timeDif -= 1f;
        timeDif *= -1f;

        return timeDif;
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
