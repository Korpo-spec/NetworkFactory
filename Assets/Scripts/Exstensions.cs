using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public static class Exstentions 
{
    public static Vector2 RandomVec2(this Vector2 vec)
    {
        vec = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        return vec.normalized;
    }
    
    public static Vector3 RandomVec3(this Vector3 vec)
    {
        vec = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        return vec.normalized;
    }

    public static Vector2 FloorToInt(this Vector2 vec)
    {
        vec.x = Mathf.FloorToInt(vec.x);
        vec.y = Mathf.FloorToInt(vec.y);
        return vec;
    }
    
    public static T GetRandom<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static bool IsCommonItem(this List<Item> items, FixedString32Bytes ID)
    {
        if (items == null || items.Count < 1)
        {
            return true;
        }
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            if (item.itemID == ID)
            {
                return true;
            }
        }

        return false;
    }

    
}
