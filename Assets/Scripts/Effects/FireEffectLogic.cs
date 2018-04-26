using System.Collections;
using UnityEngine;
using World.Objects;

public class FireEffectLogic : EffectLogic
{
    public float DamagePerTick = 3;
    public float TickRate = 0.2f;

    public override string getName()
    {
        return "Fire Spell";
    }

    public override void apply(UnitLogic player, float duration)
    {
        player.ShowPrefab("Effects\\FireEffect", duration);
        object[] param = new object[3] {duration, TickRate, DamagePerTick};
        player.ProxyCoroutine("DealDamageOverTime", param);
    }
}