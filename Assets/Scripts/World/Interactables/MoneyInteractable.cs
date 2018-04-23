using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoneyInteractable : Interactable
{
    public int Money;

    public override void interact(UnitLogic player)
    {
        if (!CanInteract(player))
        {
            return;
        }

        player.Money += Money;
        Destroy(gameObject);
    }
}