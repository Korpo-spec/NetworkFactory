using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Belt : MonoBehaviour
{
    [SerializeField] private Belt connectedBelt;

    [SerializeField]private List<BeltItemData> beltItems;

    private Queue<(Item item, float pos)> itemsToNextbelt;

    private Vector3 startPos;

    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        itemsToNextbelt = new Queue<(Item item, float pos)>();
        beltItems = new List<BeltItemData>();
        startPos = transform.position + transform.up * -(transform.localScale.x / 2);
        direction = transform.up * (transform.localScale.x);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < beltItems.Count; i++)

        {
            var item = beltItems[i];
            float timeDif = (item.pos-TickClock.instance.clock) / 2.0f;
            timeDif -= 1f;
            timeDif *= -1f;
            //Debug.Log(timeDif);
            if (timeDif > 1f)
            {
                Debug.Log("Diff", item.item);
                if (!connectedBelt)
                {
                    continue;
                }
                bool space = connectedBelt.AddItemToBelt(item.item, timeDif - 1.0f);
                if (space)
                {
                    Debug.Log("moved Item to next belt");
                    beltItems.Remove(item);
                    i--;
                }
                continue;
            } 

            

            if (i > 0 && (beltItems[i-1].item.transform.position - item.item.transform.position).sqrMagnitude < 1.1f)
            {
                Debug.Log("Stuck", item.item);
                item.item.transform.position = beltItems[i - 1].item.transform.position - direction.normalized;
                float thingy = item.pos - TickClock.instance.clock;
                item.pos = TickClock.instance.clock + thingy;
            }
            else
            {
                item.item.transform.position = startPos + (direction * timeDif);
            }
            
            
            //item.item.transform.position = startPos + (direction * timeDif);
        }
        
        
        //itemsToNextbelt.Clear();
    }

    public bool AddItemToBelt(Item item, float position)
    {
        
        if (beltItems.Count > 0)
        {
            if ((beltItems[beltItems.Count-1].item.transform.position - item.transform.position).sqrMagnitude < 1.1f)
            {
                Debug.Log("Tooclose");
                return false;
            }
        }

        if (beltItems.Count >= 4)
        {
            return false;
        }
        Debug.Log("AddITEM");
        item.transform.position = transform.position + transform.up * -(transform.localScale.x / 2);
        float endTime = TickClock.instance.clock + 2.0f;
        beltItems.Add(new BeltItemData(item, endTime));
        return true;
    }

    public bool externalAddItemToBelt(Item item, float position)
    {
        item.transform.position = transform.position + transform.up * -(transform.localScale.x / 2);
        if (beltItems.Count > 0)
        {
            if ((beltItems[beltItems.Count-1].item.transform.position - item.transform.position).sqrMagnitude < 1.0f)
            {
                Debug.Log("Tooclose");
                return false;
            }
        }
        Debug.Log("AddITEM");
        item.transform.position = transform.position + transform.up * -(transform.localScale.x / 2);
        float endTime = TickClock.instance.clock + 2.0f- position;
        beltItems.Add(new BeltItemData(item, endTime));
        return true;
        
    }
}
