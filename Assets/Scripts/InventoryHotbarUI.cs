using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHotbarUI : MonoBehaviour
{
    private InventoryHotbarIcon[] icons;

    [SerializeField] private Color selectColor;

    private Color originalColor = Color.white;

    private int lastSelected = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        icons = GetComponentsInChildren<InventoryHotbarIcon>();
    }

    void Start()
    {
        originalColor = transform.GetChild(0).GetComponent<Image>().color;
        icons[lastSelected].ChangeColor(selectColor);
    }

    public Item GetHotbarItem(int slot)
    {
        if (slot >= icons.Length)
        {
            Debug.LogError("THERE ARE NOT THAT MANY SLOTS");
            return null;
        }

        icons[lastSelected].ChangeColor(originalColor);
        icons[slot].ChangeColor(selectColor);
        
        lastSelected = slot;
        
        
        return icons[slot].item;
    }
}
