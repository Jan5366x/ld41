using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickupInteractable : Interactable
{
    public GameObject obj;
    public override void interact(UnitLogic player)
    {
        if (!CanInteract(player))
        {
            return;
        }
        
        player.Inventory.Take(obj);
        gameObject.SetActive(false);
    }
}