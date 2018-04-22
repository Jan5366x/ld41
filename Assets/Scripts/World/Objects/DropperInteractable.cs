using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct RandomDrop
{
    public GameObject item;
    public float probability;
}

public class DropperInteractable : Interactable
{
    public GameObject[] GuaranteedDrop;
    public RandomDrop[] RandomDrop;
    public int TriggerCount = 1;
    public float SpreadX = 0.32f;
    public float SpreadY = 0.32f;

    public override void interact(UnitLogic player)
    {
        if (!CanInteract(player))
        {
            return;
        }

        if (GuaranteedDrop != null)
        {
            foreach (var drop in GuaranteedDrop)
            {
                SpawnItem(drop);
            }
        }

        if (RandomDrop != null)
        {
            foreach (var drop in RandomDrop)
            {
                if (Random.value < drop.probability)
                {
                    SpawnItem(drop.item);
                }
            }
        }

        TriggerCount -= 1;
    }

    public override bool CanInteract(UnitLogic obj)
    {
        return base.CanInteract(obj) && TriggerCount >= 0;
    }

    private void SpawnItem(GameObject obj)
    {
        float dx = (Random.value * SpreadX) - SpreadX / 2f;
        float dy = (Random.value * SpreadY) - SpreadY / 2f;

        var i = Instantiate(obj, transform);
        i.transform.Translate(dx, dy, 0);
    }
}