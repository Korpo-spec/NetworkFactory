using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace DefaultNamespace
{
    public class Building : NetworkBehaviour
    {
        public virtual Vector2 size { get; }
        
        public virtual NetworkObject NetworkObject { get; }

        public FixedString32Bytes buildingID;

        public virtual void OnPlace()
        {
            
        }
    }
}