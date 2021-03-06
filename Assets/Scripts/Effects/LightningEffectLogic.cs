﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World.Objects;

public class LightningEffectLogic : EffectLogic
{
    public float MinDamage = 3;
    public float MaxDamage = 10;

    public override string getName()
    {
        return "Lightning Spell";
    }

    public override void apply(UnitLogic player, float duration)
    {
        player.ShowPrefab("Effects\\LightningEffect", duration);
        player.ReceiveDamage(Random.value * (MaxDamage - MinDamage) + MinDamage);
        player.Stamina = 0;
    }
}