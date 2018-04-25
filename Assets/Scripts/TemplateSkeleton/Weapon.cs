using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float Damage = 10;
    public float Range = 5;
    public float CoolDown = 1;
    public EffectLogic Effect;
    public float EffectDuration;
    public bool Multi = false;
    
    public bool Magic = false;
    public GameObject Projectile;
    public float ProjectileSpeed = 1;
    public float ManaUsage = 0;

    private void Start()
    {
        
    }
}