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
        player.ShowPrefab("Effects\\IceEffect", duration);
        object[] param = new object[1] {duration};
        player.StartCoroutine("Freeze", param);
    }
}