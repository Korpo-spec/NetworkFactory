using System.Collections.Generic;

namespace DefaultNamespace
{
    public interface IInserterInteract
    {
        public bool wantItemGround => false;
        public Item GetItem(ref int amount, List<Item> neededItems);

        public bool AddItem(ItemGround item, int amount)
        {
            return false;
        }
        public bool AddItem(Item item, int amount)
        {
            return false;
        }
        
        

        public List<Item> GetNeededItems()
        {
            return null;
        }

        public static ItemGround itemGround;


    }
}