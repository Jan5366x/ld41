using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ViewInventory : MonoBehaviour
{
    public abstract void handleSelectSlot(int slot);

    private Inventory _inventory;

    public Inventory Inventory
    {
        get { return _inventory.Copy(); }
        set { _inventory = value.Copy(); }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}