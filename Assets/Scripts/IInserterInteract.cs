namespace DefaultNamespace
{
    public interface IInserterInteract
    {
        public Item GetItem(ref int amount);

        public bool AddItem(ItemGround item, int amount);

        public static ItemGround itemGround;


    }
}