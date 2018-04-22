using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public const int HEAD_SLOT = 0;
    public const int BODY_SLOT = 1;
    public const int LEG_SLOT = 2;
    public const int FOOT_SLOT = 3;
    public const int HAND_LEFT_SLOT = 4;
    public const int HAND_RIGHT_SLOT = 5;
    public const int OFFSET_SLOT = 6;
    public const int COUNT_SLOT = 20;

    public InventoryItem[] Items;

    // Use this for initialization
    public void Start()
    {
        Items = new InventoryItem[COUNT_SLOT];
        for (int i = 0; i < COUNT_SLOT; i++)
        {
            Items[i] = gameObject.AddComponent<InventoryItem>();
        }
    }

    // Update is called once per frame
    public void Update()
    {
    }

    public void Equip(int slot, GameObject obj)
    {
        if (slot < 0 || slot >= COUNT_SLOT)
        {
            return;
        }

        if (Items[slot].quantity == 0)
        {
            Items[slot].obj = obj;
            Items[slot].quantity = 1;
        }
        else
        {
            if (Items[slot].obj == obj)
            {
                Items[slot].quantity += 1;
            }
        }
    }

    public void Unequip(int slot)
    {
        if (slot < 0 || slot >= COUNT_SLOT)
        {
            return;
        }

        Items[slot].obj = null;
        Items[slot].quantity = slot;
    }

    public void Swap(int slotA, int slotB)
    {
        GameObject tmpObj = Items[slotA].obj;
        int quantity = Items[slotA].quantity;

        Items[slotA].obj = Items[slotB].obj;
        Items[slotA].quantity = Items[slotB].quantity;
        Items[slotB].obj = tmpObj;
        Items[slotB].quantity = quantity;
    }

    public GameObject GetObject(int slot)
    {
        if (slot < 0 || slot >= COUNT_SLOT)
        {
            return null;
        }

        return Items[slot].obj;
    }

    public int GetQuantity(int slot)
    {
        if (slot < 0 || slot >= COUNT_SLOT)
        {
            return -1;
        }

        return Items[slot].quantity;
    }
    
    public Inventory Copy()
    {
        Inventory iv = gameObject.AddComponent<Inventory>();
        for (int i = 0; i < COUNT_SLOT; i++)
        {
            iv.Items[i] = Items[i].Copy();
        }

        return iv;
    }
}