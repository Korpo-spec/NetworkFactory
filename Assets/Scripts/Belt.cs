using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.Netcode;
using UnityEngine;

public class Belt : Building
{
    [SerializeField] private Belt connectedBelt;
    
    [SerializeField] private List<Transform> beltPoints = new List<Transform>();

    private int beltPointIndex = 0;

    private int beltPointAmount = 0;

    private List<BeltItemData> beltItems;

   

    private Vector3 startPos;

    public Vector3 direction;

    
    public override Vector2 size => Vector2.one;
    public override NetworkObject NetworkObject => GetComponent<NetworkObject>();

    
    // Start is called before the first frame update
    void Start()
    {
        
        beltItems = new List<BeltItemData>();
        startPos = transform.position + transform.up * -(transform.localScale.x / 2);
        direction = transform.up ;
        beltPointAmount = beltPoints.Count - 1;
    }

    // Update is called once per frame
    void Update()
    {
        float prevTimeDif = 0;
        for (int i = 0; i < beltItems.Count; i++)
        {
            var item = beltItems[i];
            float timeDif = (item.pos-TickClock.instance.clock) / 2.0f;
            timeDif -= 1f;
            timeDif *= -1f;

            timeDif = Mathf.Min(timeDif, 1.0001f);
            //Debug.Log(timeDif);
            if (timeDif > 1f)
            {
                prevTimeDif = timeDif;
                //Debug.Log("Diff", item.item);
                if (!connectedBelt)
                {
                    continue;
                }
                bool space = connectedBelt.AddItemToBelt(item.item, timeDif - 1.0f);
                if (space)
                {
                    //Debug.Log("moved Item to next belt");
                    beltItems.Remove(item);
                    i--;
                }
                continue;
            } 

            

            if (i > 0 && (prevTimeDif - timeDif) < 0.25f)
            {
                //Debug.Log("Stuck " + prevTimeDif + " " + timeDif, item.item);
                //item.item.transform.position = beltItems[i - 1].item.transform.position - direction.normalized;
                //float thingy = item.pos - TickClock.instance.clock;
                //Debug.Log(item.pos);
                beltItems[i] = new BeltItemData(item.item, item.pos + (0.25f-(prevTimeDif - timeDif)));
                //Debug.Log(item.pos);
            }


            if (timeDif * beltPointAmount > 1.0f)
            {
                beltPointIndex = 1;
            }
            else
            {
                beltPointIndex = 0;
            }
            item.item.transform.position = Vector3.Lerp(beltPoints[beltPointIndex].position, beltPoints[beltPointIndex+1].position, (timeDif*beltPointAmount)%1);

            prevTimeDif = timeDif;
            

            //item.item.transform.position = startPos + (direction * timeDif);
        }
        
        
        //itemsToNextbelt.Clear();
    }

    public bool AddItemToBelt(ItemGround item, float position)
    {
        
        if (beltItems.Count > 0)
        {
            if ((beltItems[beltItems.Count-1].item.transform.position - item.transform.position).sqrMagnitude < 0.25f)
            {
                Debug.Log("Tooclose");
                return false;
            }
        }

        if (beltItems.Count >= 4)
        {
            return false;
        }
        //Debug.Log("AddITEM");
        item.transform.position = transform.position + transform.up * -(transform.localScale.x / 2);
        float endTime = TickClock.instance.clock + 2.0f;
        beltItems.Add(new BeltItemData(item, endTime));
        return true;
    }

    public bool externalAddItemToBelt(ItemGround item, float position)
    {
        item.transform.position = (transform.position + transform.up * -(transform.localScale.x / 2)) + (direction*position);
        if (beltItems.Count > 0)
        {
            if ((beltItems[beltItems.Count-1].item.transform.position - item.transform.position).sqrMagnitude < 0.25f)
            {
                Debug.Log("Tooclose");
                return false;
            }
        }
        //Debug.Log("AddITEM");
        item.transform.position = transform.position + transform.up * -(transform.localScale.x / 2);
        float endTime = TickClock.instance.clock + 2.0f- position* 2.0f;
        beltItems.Add(new BeltItemData(item, endTime));
        return true;
        
    }

    public override void OnPlace(Map map, Vector2 pos)
    {
        var g = (map.map.GetValue(pos + (Vector2) transform.up));
        if (g)
        {
            if (g.TryGetComponent<Belt>(out Belt belt))
            {
                connectedBelt = belt;
            }
        }

        g = map.map.GetValue(pos + (Vector2) transform.up * -1);
        if (g)
        {
            if (g.TryGetComponent<Belt>(out Belt belt))
            {
               belt.connectedBelt = this;
            }
        }
        
        
        
        
    }

    public override Building OnPlaceServerSide(Map map, Vector2 pos)
    {
        return this;
    }
}
