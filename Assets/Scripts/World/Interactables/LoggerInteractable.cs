using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoggerInteractable : Interactable
{
    public override void interact(UnitLogic player)
    {
        if (CanInteract(player))
        {
            Debug.Log(player);
        }
    }
}