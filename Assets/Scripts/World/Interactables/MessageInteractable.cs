using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MessageInteractable : Interactable
{
    public String firstInteraction;
    public String laterInteraction;

    public int interactionCount = 0;

    public override void interact(UnitLogic player)
    {
        if (!CanInteract(player))
        {
            return;
        }

        if (interactionCount == 0)
        {
            player.ShowMessage(firstInteraction, 10);
        }
        else
        {
            player.ShowMessage(firstInteraction, 10);
        }

        interactionCount++;
    }
}