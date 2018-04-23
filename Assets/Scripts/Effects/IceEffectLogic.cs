using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEffectLogic : EffectLogic
{
    public override string getName()
    {
        return "Ice Spell";
    }

    public override void apply(UnitLogic player, float duration)
    {
        showPrefab("Effects\\IceEffect", duration);
        object[] param = new object[2] {player, duration};
        StartCoroutine("FreezePlayer", param);
    }

    private IEnumerator FreezePlayer(object[] param)
    {
        UnitLogic player = (UnitLogic) param[0];
        float duration = (float) param[1];

        var oldCooldown = player.CoolDown;
        player.CoolDown = 99999;
        player.MaxSpeed = 0;
        yield return new WaitForSeconds(duration);
        player.CoolDown = oldCooldown - duration;
        player.MaxSpeed = player.Template.MaxSpeed;
    }
}