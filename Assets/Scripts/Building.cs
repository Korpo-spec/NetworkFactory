using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace DefaultNamespace
{
    public class Building : NetworkBehaviour
    {
        public virtual Vector2 size => Vector2.one;

        public virtual NetworkObject NetworkObject => GetComponent<NetworkObject>();

        public FixedString32Bytes buildingID;

        public bool rotatable = false;

        public virtual Building OnPlaceServerSide(Map map, Vector2 pos, Quaternion rot)
        {
            return this;
        }
        public virtual void OnPlace(Map map, Vector2 pos)
        {
            
        }
    }
}