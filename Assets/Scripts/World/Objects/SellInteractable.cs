using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SellInteractable : Interactable
{
    public Inventory inventory;

    public override void interact(UnitLogic player)
    {
        if (!CanInteract(player))
        {
            return;
        }

        player.SellInventory(inventory);
    }
}