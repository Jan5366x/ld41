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
        showPrefab("Effects\\PoisonEffect", duration);
        object[] param = new object[2] {player, duration};
        StartCoroutine("dealDamage", param);
    }

    private IEnumerator dealDamage(object[] param)
    {
        UnitLogic player = (UnitLogic) param[0];
        float duration = (float) param[1];
        
        player.MaxSpeed = 0;
        player.Stamina = 0;
        int numTicks = (int) (duration / TickRate);
        for (int i = 0; i < numTicks; i++)
        {
            player.ReceiveDamage(DamagePerTick);
            yield return new WaitForSeconds(TickRate);
        }
        player.MaxSpeed = player.Template.MaxSpeed;
    }
}