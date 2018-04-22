﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Array = UnityScript.Lang.Array;
using Random = UnityEngine.Random;

public class UnitLogic : MonoBehaviour
{
    public Unit Template;

    public float HP;
    public float Mana;
    public float MaxSpeed;
    public float CoolDown;

    public GameObject _presentation;
    public GameObject _target;
    public GameObject _targetMarker;
    public Inventory _inventory;

    private Dictionary<EffectLogic, float> activeEffects = new Dictionary<EffectLogic, float>();

    // Use this for initialization

    void Start()
    {
        // instantiate the presentation object
        _presentation = Instantiate(Template.Presentation, transform);
        _targetMarker = Instantiate(Template.TargetMarker, transform);
        _inventory = gameObject.AddComponent<Inventory>();

        HP = Template.MaxHealth;
        Mana = Template.MaxMana;
        MaxSpeed = Template.MaxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        print("+++" + activeEffects);
        foreach (var effect in activeEffects.Keys)
        {
            effect.apply(this);
            activeEffects[effect] -= Time.deltaTime;
        }

        activeEffects = activeEffects.Where(pair => pair.Value > 0)
            .ToDictionary(item => item.Key, value => value.Value);
        CoolDown -= Time.deltaTime;

        HP = Mathf.Min(HP + Template.HPRegeneration * Time.deltaTime, Template.MaxHealth);
        Mana = Mathf.Min(Mana + Template.ManaRegeneration * Time.deltaTime, Template.MaxMana);
    }

    public GameObject Presentation
    {
        get { return _presentation; }
    }

    public void Move(float dx, float dy, bool sprint)
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        if (Mathf.Abs(dx) > Mathf.Abs(dy))
        {
            dy = 0;
        }
        else if (Mathf.Abs(dx) < Mathf.Abs(dy))
        {
            dx = 0;
        }

        dx *= Template.Acceleration;
        dy *= Template.Acceleration;

        if (sprint)
        {
            dx *= (1 + Random.value * 0.2f);
            dy *= (1 + Random.value * 0.2f);
        }

        body.AddForce(new Vector2(dx, dy));
    }

    public void Interact()
    {
        var hits = Physics2D.CircleCastAll(new Vector2(transform.position.x, transform.position.y), Template.HandRange,
            new Vector2(0, 0));

        int minIdx = -1;
        float minDist = 99999;
        int idx = 0;
        Interactable bestInteractable = null;

        foreach (var hit in hits)
        {
            var collider = hit.collider;
            var interactable = collider.GetComponentInChildren<Interactable>();
            float dist = (collider.transform.position - transform.position).sqrMagnitude;
            if (dist < minDist && interactable != null)
            {
                minDist = dist;
                minIdx = idx;
                bestInteractable = interactable;
            }

            idx++;
        }

        if (minIdx >= 0 && bestInteractable != null)
        {
            if (bestInteractable.CanInteract(this))
            {
                bestInteractable.interact(this);
            }
        }
        else
        {
            SwitchTarget();
        }
    }

    public void AttackLeft()
    {
        var item = _inventory.GetObject(Inventory.HAND_LEFT_SLOT);
        Attack(item);
    }

    public void AttackRight()
    {
        var item = _inventory.GetObject(Inventory.HAND_RIGHT_SLOT);
        Attack(item);
    }
    
    public void AttackLeft(UnitLogic unit)
    {
        var item = _inventory.GetObject(Inventory.HAND_LEFT_SLOT);
        Attack(unit, item);
    }

    public void AttackRight(UnitLogic unit)
    {
        var item = _inventory.GetObject(Inventory.HAND_RIGHT_SLOT);
        Attack(unit, item);
    }

    private void Attack(GameObject weapon)
    {
        if (CoolDown > 0)
        {
            return;
        }

        var damageScript = weapon == null ? null : weapon.GetComponent<Weapon>();
        var units = getHitUnits(damageScript);
        foreach (var unit in units)
        {
            AttackUnit(unit, damageScript);
        }

        CoolDown = damageScript == null ? 1 : damageScript.CoolDown;
    }
    
    private void Attack(UnitLogic unit, GameObject weapon)
    {
        if (CoolDown > 0)
        {
            return;
        }

        var damageScript = weapon == null ? null : weapon.GetComponent<Weapon>();
        if (unit)
        {
            AttackUnit(unit, damageScript);
        }
        var units = getHitUnits(damageScript);
        
        CoolDown = damageScript == null ? 1 : damageScript.CoolDown;
    }

    public void AttackUnit(UnitLogic other, Weapon weapon)
    {
        if (other && CoolDown <= 0)
        {
            if (weapon && weapon.Magic)
            {
                FireProjectile(weapon, other);
            }
            else
            {
                HitMeele(weapon, other);
            }
        }
    }

    private void HitMeele(Weapon weapon, UnitLogic target)
    {
        if (target)
        {
            float damage = weapon != null ? weapon.Damage : 1;
            EffectLogic effect = weapon != null ? weapon.Effect : null;
            float effectDuration = weapon != null ? weapon.EffectDuration : 0;
            target.ReceiveDamage(damage);
            target.ReceiveEffect(effect, effectDuration);
        }
    }

    private void FireProjectile(Weapon weapon, UnitLogic target)
    {
        if (weapon == null)
            return;

        var projectile = Instantiate(weapon.Projectile, transform);
        var projectileScript = projectile.GetComponentInChildren<ProjectileLogic>();
        projectileScript.Target = target.gameObject;
        projectileScript.Weapon = weapon;
    }

    private UnitLogic[] getHitUnits(Weapon weapon)
    {
        Array hitUnits = new Array();
        if (weapon != null && weapon.Magic)
        {
            hitUnits.Add(_target);
        }
        else
        {
            var range = weapon != null ? weapon.Range : Template.HandRange;
            var multipleHits = weapon != null ? weapon.Multi : false;
            var hits = Physics2D.CircleCastAll(new Vector2(transform.position.x, transform.position.y), range,
                new Vector2(0, 0));

            float minDist = 99999;
            UnitLogic nearestUnit = null;
            foreach (var hit in hits)
            {
                var collider = hit.collider;
                var hitUnit = collider.GetComponentInChildren<UnitLogic>();
                float dist = (collider.transform.position - transform.position).sqrMagnitude;

                if (!hitUnit)
                {
                    continue;
                }

                if (!hitUnit.Template.IsEnemy)
                {
                    continue;
                }

                if (Random.value < hitUnit.Template.Agility / 100f)
                {
                    continue;
                }

                if (hitUnit == this)
                {
                    continue;
                }

                if (multipleHits)
                {
                    hitUnits.Add(hitUnit);
                }
                else if (dist < minDist)
                {
                    minDist = dist;
                    nearestUnit = hitUnit;
                }
            }

            if (!multipleHits)
            {
                hitUnits.Add(nearestUnit);
            }
        }

        return hitUnits.Cast<UnitLogic>().ToArray();
    }

    public void ReceiveDamage(float damage)
    {
        if (IsDead())
            return;
        
        HP -= damage;
        if (IsDead())
        {
            Die();
        }

        print(GetComponentInChildren<ShowDamage>());

        GetComponentInChildren<ShowDamage>().Show(damage);
    }

    public void Die()
    {
        _presentation = Instantiate(Template.DeathPrefab, transform);
        Debug.LogError("I Should be dead by now");
    }

    public void SwitchTarget()
    {
        var range = GetMaxWeaponRange();
        var unitsInRange = Physics2D.CircleCastAll(new Vector2(transform.position.x, transform.position.y), range * 10,
            new Vector2(0, 0));
        Array targets = new Array();


        for (int i = 0; i < unitsInRange.Length; i++)
        {
            var collider = unitsInRange[i].collider;
            var hitUnit = collider.GetComponentInChildren<UnitLogic>();
            if (!hitUnit)
            {
                continue;
            }

            if (!hitUnit.Template.IsEnemy)
            {
                continue;
            }

            if (hitUnit.gameObject == this || hitUnit.gameObject == _target)
            {
                continue;
            }

            targets.Add(collider.gameObject);
        }

        if (targets.length > 0)
        {
            int idx = (int) (Random.value * targets.length);
            SetTarget((GameObject) targets[idx]);
        }
    }

    public float GetWeaponRangeLeft()
    {
        var item = _inventory.GetObject(Inventory.HAND_LEFT_SLOT);
        var weapon = item != null ? item.GetComponent<Weapon>() : null;
        var range = weapon != null ? weapon.Range : Template.HandRange;
        return range;
    }

    public float GetWeaponRangeRight()
    {
        var item = _inventory.GetObject(Inventory.HAND_RIGHT_SLOT);
        var weapon = item != null ? item.GetComponent<Weapon>() : null;
        var range = weapon != null ? weapon.Range : Template.HandRange;
        return range;
    }

    public float GetMaxWeaponRange()
    {
        return Mathf.Max(GetWeaponRangeLeft(), GetWeaponRangeRight());
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
        _targetMarker.transform.SetParent(_target.transform, false);
    }

    public void ReceiveEffect(EffectLogic weaponEffect, float weaponEffectDuration)
    {
        if (weaponEffect && weaponEffectDuration > 0)
        {
            if (!activeEffects.ContainsKey(weaponEffect))
            {
                activeEffects.Add(weaponEffect, 0);
            }

            activeEffects[weaponEffect] += weaponEffectDuration;
        }
    }

    public bool IsDead()
    {
        return HP < 0;
    }
}