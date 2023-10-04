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

        public bool rotatable = false;

        public virtual Building OnPlaceServerSide(Map map, Vector2 pos)
        {
            return this;
        }
        public virtual void OnPlace(Map map, Vector2 pos)
        {
            
        }
    }
}