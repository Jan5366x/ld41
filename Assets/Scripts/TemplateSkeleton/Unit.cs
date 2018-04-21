using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Template/Unit")]
public class Unit : ScriptableObject
{
    public string UnitName;
    
    public int MaxHealth;
    public int MaxMana;
    
    public int Strength;
    public int Intelligence;
    public int Stamina;
    public int Agility;
    
    public float MaxSpeed;
    public float Acceleration;
    
    public bool IsEnemy;
    public bool IsInteractable;
    public bool IsShowName;
    
    public GameObject Presentation;
}
