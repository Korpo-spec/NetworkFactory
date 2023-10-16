using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Unity.Netcode;
using UnityEngine;

public class Belt : Building, IInserterInteract
{
    [SerializeField] private Belt connectedBelt;
    
    [SerializeField] private List<Transform> beltPoints = new List<Transform>();

    [SerializeField] private Belt beltVariation;
    
    [SerializeField] private Belt originalBelt;

    private int beltPointIndex = 0;

    private int beltPointAmount = 0;

    private List<BeltItemData> beltItems;

   

    private Vector3 startPos;

    public Vector3 direction;

    
    public override Vector2 size => Vector2.one;
    public override NetworkObject NetworkObject => GetComponent<NetworkObject>();

    public static Animator mainAnimator;

    private void Awake()
    {
        beltItems = new List<BeltItemData>();
        startPos = transform.position + transform.up * -(transform.localScale.x / 2);
        direction = transform.up ;
        beltPointAmount = beltPoints.Count - 1;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!mainAnimator)
        {
            mainAnimator = this.GetComponent<Animator>();
        }
        else
        {
            GetComponent<Animator>().Play(0,-1, mainAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
       
        
    }

    // Update is called once per frame
    void Update()
    {
        float prevTimeDif = 0;
        for (int i = 0; i < beltItems.Count; i++)
        {
            
            var item = beltItems[i];
            if (!item.item)
            {
                beltItems.RemoveAt(i);
                i--;
                continue;
            }
            float timeDif = (item.pos-TickClock.instance.clock) / 2.0f;
            timeDif -= 1f;
            timeDif *= -1f;

            timeDif = Mathf.Min(timeDif, 1.0001f);
            //Debug.Log(timeDif);
            if (timeDif >= 1f)
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
                prevTimeDif = timeDif;
                continue;
                //Debug.Log(item.pos);
            }


            if (timeDif * beltPointAmount >= 1.0f)
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
            if ((beltItems[beltItems.Count-1].item.transform.position - item.transform.position).sqrMagnitude < 0.125f*0.125f)
            {
                //Debug.Log("Tooclose");
                return false;
            }
        }

        if (beltItems.Count >= 6)
        {
            return false;
        }
        //Debug.Log("AddITEM");
        //item.transform.position = transform.position + transform.up * -(transform.localScale.x / 2);
        float endTime = TickClock.instance.clock + 2.0f;
        beltItems.Add(new BeltItemData(item, endTime));
        return true;
    }

    public bool externalAddItemToBelt(ItemGround item, float position)
    {
        item.transform.position = (transform.position + transform.up * -(transform.localScale.x / 2)) + (direction*position);
        if (beltItems.Count > 0)
        {
            if ((beltItems[beltItems.Count-1].item.transform.position - item.transform.position).sqrMagnitude < 0.125f)
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
        
        

        var g = map.map.GetValue(pos + (Vector2) transform.up * -1);
        if (g)
        {
            if (g.TryGetComponent<Belt>(out Belt belt) && belt.transform.up == transform.up)
            {
               belt.connectedBelt = this;
               
            }
        }
        g = (map.map.GetValue(pos + (Vector2) transform.up));
        if (g)
        {
            if (g.TryGetComponent<Belt>(out Belt belt))
            {
                
                connectedBelt = belt;
                if (belt.CheckForTurn(map, pos, out _))
                {
                    map.ReplaceBuilding(originalBelt, belt.transform.position, belt.transform.rotation);
                    
                }
            }
        }
        
        g = map.map.GetValue(pos + (Vector2) transform.right);
        if (g)
        {
            if (g.TryGetComponent<Belt>(out Belt belt) && !(belt.transform.up == transform.up|| belt.transform.up == transform.up *-1))
            {
                belt.connectedBelt = this;
                return;
            }
        }
        
        g = map.map.GetValue(pos + (Vector2) transform.right * -1);
        if (g)
        {
            if (g.TryGetComponent<Belt>(out Belt belt)&& !(belt.transform.up == transform.up|| belt.transform.up == transform.up *-1))
            {
                belt.connectedBelt = this;
                return;
            }
        }
        
        
    }

    public override Building OnPlaceServerSide(Map map, Vector2 pos, Quaternion rot)
    {
        if (CheckForTurn(map, pos, out bool belt2))
        {
            Debug.Log("Variation spawn");
            Destroy(this.gameObject);
            var newbelt = Instantiate(beltVariation, pos - new Vector2(25, 25), rot);
            newbelt.transform.localScale = new Vector3(belt2 ? -1 : 1,1,1);
            return newbelt;
        }
        
        return this;
        
        
    }

    private bool CheckForTurn(Map map, Vector2 pos, out bool right)
    {
        right = false;
        var g = (map.map.GetValue(pos + (Vector2) transform.up * -1));
        if (g && g.transform.up == transform.up) return false;
        Debug.Log("not Same transform.up");
            
        Debug.Log(pos + (Vector2) transform.right);
        g = map.map.GetValue(pos + (Vector2) transform.right);
        var g2 = map.map.GetValue(pos + (Vector2) transform.right * -1);
            
        if (g ^ g2)
        {
            Debug.Log("FoundOnespecial");
            bool belt1 = g && g.TryGetComponent<Belt>(out _) && !(g.transform.up == transform.up|| g.transform.up == transform.up *-1);
            bool belt2 = g2 && g2.TryGetComponent<Belt>(out _) && !(g2.transform.up == transform.up|| g2.transform.up == transform.up *-1);
            right = belt2;
            if (belt1 ^ belt2)
            {
                Debug.Log("FoundOnespecialBelt");
                return true;
            }
        }

        return false;
    }

    public Item GetItem(ref int amount, List<Item> neededItems)
    {
        if (beltItems.Count < 1)
        {
            return null;
        }
        BeltItemData closestItem = new BeltItemData(null, 1);
        int index = 0;
        for (int i = 0; i < beltItems.Count; i++)
        {
            var item = beltItems[i];
            float timeDif = (item.pos-TickClock.instance.clock) / 2.0f;
            timeDif -= 1f;
            timeDif *= -1f;
            timeDif = Mathf.Min(timeDif, 1.0001f);

            float closestValue = Mathf.Abs(timeDif - 0.5f);
            Debug.Log(closestValue);
            if (closestItem.pos > closestValue)
            {
                if (neededItems ==null)
                {
                    closestItem = new BeltItemData(item.item, closestValue);
                }
                else
                {
                    for (int j = 0; j < neededItems.Count; j++)
                    {
                        if (item.item.item.itemID == neededItems[j].itemID)
                        {
                            closestItem = new BeltItemData(item.item, closestValue);
                        }
                    }
                }
                
                
                index = i;
            }
        }

        beltItems.RemoveAt(index);
        return closestItem.item.item;
    }
    

    public bool wantItemGround => true;
    public bool AddItem(ItemGround item, int amount)
    {
        return AddItemToBelt(item, 0.5f);
    }
}
