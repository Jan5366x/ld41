using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectLogic : MonoBehaviour
{
    public abstract string getName();
    public abstract void apply(UnitLogic player, float duration);

    public void showPrefab(string name, float duration)
    {
        var obj = Instantiate(Resources.Load(name), transform) as GameObject;
        var autoDestruct = obj.GetComponentInChildren<AutoDestruct>();
        if (autoDestruct)
        {
            autoDestruct.Fire(duration);
        }
    }
}