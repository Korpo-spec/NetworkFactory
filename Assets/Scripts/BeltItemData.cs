using System;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public struct BeltItemData
    {
        public ItemGround item;
        public float pos;
        public int beltPointIndex;

        public BeltItemData(ItemGround item, float endTime)
        {
            this.item = item;
            this.pos = endTime;
            beltPointIndex = 0;
        }
    }
}