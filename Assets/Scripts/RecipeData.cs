using System;

namespace DefaultNamespace
{
    [Serializable]
    public class RecipeData
    {
        public Item craftItem;
        public int amount;
        [NonSerialized]public int currentAmount;
    }
}