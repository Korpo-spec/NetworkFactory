using System;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public struct BeltItemData
    {
        public ItemGround item;
        public float pos;
        public bool stuck;

        public BeltItemData(ItemGround item, float endTime)
        {
            this.item = item;
            this.pos = endTime;
            stuck = false;
        }
    }
}