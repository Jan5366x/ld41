using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectLogic : MonoBehaviour
{
    public abstract string getName();
    public abstract void apply(UnitLogic player, float duration);
}