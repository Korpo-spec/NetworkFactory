using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Furnace : Building, IInserterInteract
{
    public override Vector2 size => Vector2.one * 2;

    private float endTime;

    private bool enoughRecources = false;

    [SerializeField] private Recipe recipe;
    private Recipe currentRecipeTracker;

    // Start is called before the first frame update
    void Start()
    {
        currentRecipeTracker = Instantiate(recipe);
    }

    // Update is called once per frame
    void Update()
    {
        if (enoughRecources && TickClock.instance.TimeLeft(endTime, recipe.craftTime) > 1.0f)
        {
            Debug.Log("Crafting");
            
            for (int i = 0; i < currentRecipeTracker.output.Count; i++)
            {
                var currentOutput = currentRecipeTracker.output[i];

                currentOutput.currentAmount += recipe.output[i].amount;
            }
            enoughRecources = false;
        }
    }

    public override NetworkObject NetworkObject => GetComponent<NetworkObject>();

    public Item GetItem(ref int amount, List<Item> neededItems)
    {
        Debug.Log("Trying to get item");
        for (int i = 0; i < currentRecipeTracker.output.Count; i++)
        {
            var currentOutput = currentRecipeTracker.output[i];

            Debug.Log(currentOutput.currentAmount);
            if (currentOutput.currentAmount > 0 && neededItems.IsCommonItem(currentOutput.craftItem.itemID))
            {
                currentOutput.currentAmount -= 1;
                return currentOutput.craftItem;
            }
            
        }

        return null;
    }

    public bool AddItem(Item item, int amount)
    {
        
        for (int i = 0; i < currentRecipeTracker.input.Count; i++)
        {
            var inputData = currentRecipeTracker.input[i];
            if (inputData.craftItem.itemID == item.itemID)
            {
                if (inputData.currentAmount >= recipe.input[i].amount)
                {
                    return false;
                }
                //AddItemClientRpc(i, amount);
                currentRecipeTracker.input[i].currentAmount += amount;
                SendIfEnoughItemsClientRpc(CompileRecipeData(), TickClock.instance.clock);
                return true;
            }
        }

        return false;
        
        throw new System.NotImplementedException();
    }

    [ClientRpc]
    private void AddItemClientRpc(int index, int amount)
    {
        currentRecipeTracker.input[index].currentAmount += amount;
    }

    private FixedString32Bytes CompileRecipeData()
    {
        FixedString32Bytes returnString = new FixedString32Bytes();
        for (int i = 0; i < currentRecipeTracker.input.Count; i++)
        {
            var inputData = currentRecipeTracker.input[i];

            returnString += inputData.currentAmount.ToString() + ";";
        }

        return returnString;
    }

    private float lastTimeStamp;
    [ClientRpc]
    private void SendIfEnoughItemsClientRpc(FixedString32Bytes amounts, float timeStamp)
    {
        Debug.Log("DataRecieved");
        if (lastTimeStamp > timeStamp)
        {
            return;
        }
        Debug.Log("Timestamp OK");
        lastTimeStamp = timeStamp;

        string[] amountsInString = amounts.ToString().Split(';');
        bool enoughItems = true;

        for (int i = 0; i < currentRecipeTracker.input.Count; i++)
        {
            var inputData = currentRecipeTracker.input[i];
            inputData.currentAmount = int.Parse(amountsInString[i]);

            enoughItems = enoughItems && inputData.currentAmount >= inputData.amount;
        }
        
        
        if (!enoughRecources && enoughItems)
        {
            Debug.Log("ConsumingItems");
            ConsumeItems();
            enoughRecources = enoughItems;
        }
        
        
    }

    private void ConsumeItems()
    {
        for (int i = 0; i < currentRecipeTracker.input.Count; i++)
        {
            var inputData = currentRecipeTracker.input[i];
            inputData.currentAmount -= inputData.amount;
            
        }
        
        endTime = TickClock.instance.clock + currentRecipeTracker.craftTime;
    }
    
    

    public List<Item> GetNeededItems()
    {
        List<Item> neededItems = new List<Item>();

        for (int i = 0; i < currentRecipeTracker.input.Count; i++)
        {
            neededItems.Add(currentRecipeTracker.input[i].craftItem);
        }

        return neededItems;
    }
}
