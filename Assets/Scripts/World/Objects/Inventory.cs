using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public const int HEAD_SLOT = 0;
    public const int BODY_SLOT = 1;
    public const int LEG_SLOT = 2;
    public const int BOOT_SLOT = 3;
    public const int HAND_LEFT_SLOT = 4;
    public const int HAND_RIGHT_SLOT = 5;
    public const int OFFSET_SLOT = 6;
    public const int COUNT_SLOT = 20;

    public InventoryItem[] Items;
    public bool PresentationChanged;

    // Use this for initialization
    public Inventory()
    {
        Items = new InventoryItem[COUNT_SLOT];
        for (int i = 0; i < COUNT_SLOT; i++)
        {
            Items[i] = new InventoryItem();
        }

        PresentationChanged = false;
    }

    public void Equip(int slot, UnitLogic player)
    {
        if (slot < 0 || slot >= COUNT_SLOT)
        {
            return;
        }

        var obj = Items[slot];
        if (obj == null || obj.Obj == null)
        {
            return;
        }

        bool wasEquipped = false;
        for (int _slot = 0; _slot < OFFSET_SLOT; _slot++)
        {
            var equip = obj.Obj.GetComponentInChildren<EquipSlot>();
            if (equip == null || !equip.CanEquip(_slot))
            {
                continue;
            }

            Swap(slot, _slot);
            wasEquipped = true;
            break;
        }

        if (!wasEquipped)
        {
            var use = obj.Obj.GetComponentInChildren<UseSlot>();
            if (use != null)
            {
                use.OnUse(player);
                obj.Quantity -= 1;
                if (obj.Quantity == 0)
                {
                    obj.Obj = null;
                }
            }
        }
        else
        {
            PresentationChanged = true;
        }
    }

    public void Unequip(int slot)
    {
        if (slot < 0 || slot >= COUNT_SLOT)
        {
            return;
        }

        for (int _slot = OFFSET_SLOT; _slot < COUNT_SLOT; _slot++)
        {
            if (Items[_slot].Obj == null)
            {
                Items[_slot] = Items[slot].Copy();
                Items[slot].Obj = null;
                Items[slot].Quantity = 0;
                PresentationChanged = true;
                return;
            }
        }
    }

    public void Take(GameObject obj)
    {
        for (int slot = 0; slot < COUNT_SLOT; slot++)
        {
            if (slot < OFFSET_SLOT)
            {
                var equip = obj.GetComponentInChildren<EquipSlot>();
                if (equip == null || !equip.CanEquip(slot))
                {
                    continue;
                }

                if (equip.CanEquip(slot))
                {
                    Unequip(slot);
                }
            }

            if (Items[slot].Quantity == 0)
            {
                Items[slot].Obj = obj;
                Items[slot].Quantity = 1;
                return;
            }
            else
            {
                if (Items[slot].Obj == obj)
                {
                    Items[slot].Quantity += 1;
                    return;
                }
            }
        }
    }

    public void Take(InventoryItem item)
    {
        if (item != null)
        {
            Take(item.Obj);
        }
    }

    public void Drop(int slot)
    {
        if (slot < 0 || slot >= COUNT_SLOT)
        {
            return;
        }

        Items[slot].Obj = null;
        Items[slot].Quantity = slot;
    }

    public void Drop(int slot, int count)
    {
        if (slot < 0 || slot >= COUNT_SLOT)
        {
            return;
        }

        Items[slot].Quantity -= count;
        if (Items[slot].Quantity <= 0)
        {
            Items[slot].Obj = null;
        }
    }

    public void Swap(int slotA, int slotB)
    {
        GameObject tmpObj = Items[slotA].Obj;
        int quantity = Items[slotA].Quantity;

        Items[slotA] = Items[slotB].Copy();
        Items[slotB] = Items[slotB].Copy();
        Items[slotB].Obj = tmpObj;
        Items[slotB].Quantity = quantity;
    }

    public GameObject GetObject(int slot)
    {
        if (slot < 0 || slot >= COUNT_SLOT)
        {
            return null;
        }

        return Items[slot].Obj;
    }

    public int GetQuantity(int slot)
    {
        if (slot < 0 || slot >= COUNT_SLOT)
        {
            return -1;
        }

        return Items[slot].Quantity;
    }

    public Item GetItem(int slot)
    {
        if (slot < 0 || slot >= COUNT_SLOT)
        {
            return null;
        }

        var obj = Items[slot].Obj;
        if (obj == null)
            return null;

        var script = obj.GetComponent<ItemLogic>();
        if (script == null)
            return null;
        return script.Template;
    }

    public Inventory Copy()
    {
        Inventory iv = new Inventory();
        for (int i = 0; i < COUNT_SLOT; i++)
        {
            iv.Items[i] = Items[i].Copy();
        }

        return iv;
    }
}