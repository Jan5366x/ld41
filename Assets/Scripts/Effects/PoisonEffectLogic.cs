using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffectLogic : EffectLogic
{
    public float DamagePerTick = 3;
    public float TickRate = 0.2f;

    public override string getName()
    {
        return "Poison Spell";
    }

    public override void apply(UnitLogic player, float duration)
    {
        player.ShowPrefab("Effects\\PoisonEffect", duration);
        object[] param = new object[3] {duration, TickRate, DamagePerTick};
        player.ProxyCoroutine("DealDamageOverTime", param);
    }
}