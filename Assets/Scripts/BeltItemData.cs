using System;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public struct BeltItemData
    {
        public Item item;
        public float pos;
        public bool stuck;

        public BeltItemData(Item item, float endTime)
        {
            this.item = item;
            this.pos = endTime;
            stuck = false;
        }
    }
}