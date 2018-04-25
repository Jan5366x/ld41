using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectLogic : MonoBehaviour
{
    public abstract string getName();
    public abstract void apply(UnitLogic player, float duration);

    public void showPrefab(string name, float duration)
    {
        var res = Resources.Load(name);
        if (res == null)
        {
            return;
        }

        var obj = Instantiate(res, transform) as GameObject;
        if (obj == null) 
            return;
        var autoDestruct = obj.GetComponentInChildren<AutoDestruct>();
        if (autoDestruct)
        {
            autoDestruct.Fire(duration);
        }
    }
}