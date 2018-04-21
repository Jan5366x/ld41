using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Template/Effect")]
public class Effect : ScriptableObject {

    public int AdjustHealth;
    public int AdjustMana;
    public int AdjustMaxSpeed;
    public int AdjustAcceleration;
	
    public int AdjustStrength;
    public int AdjustIntelligence;
    public int AdjustStamina;
    public int AdjustAgility;
	
    public float Duration;
    public float IsPermanent;
}
