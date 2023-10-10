using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHotbarIcon : MonoBehaviour
{
    public Item item
    {
        get
        {
            return hotbarItem;
        }
        set
        {
            childRenderer.sprite = value ? value.icon: null;
            childRenderer.color = value ? Color.white : Color.clear;
            hotbarItem = value;
        }
    }

    [SerializeField] private Item hotbarItem;

    private Image childRenderer;

    private Image thisRenderer;
    // Start is called before the first frame update
    private void Awake()
    {
        childRenderer = transform.GetChild(0).GetComponentInChildren<Image>();
        thisRenderer = GetComponent<Image>();
    }
    

    public void ChangeColor(Color color)
    {
        thisRenderer.color = color;
        
    }

    void Start()
    {
        item = hotbarItem;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
