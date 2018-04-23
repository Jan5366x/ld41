using System;
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
    public float Stamina;
    public bool StaminaEmpty;
    public float CoolDown;

    public GameObject Presentation;
    public GameObject Target;
    public GameObject TargetMarker;
    public Inventory Inventory;

    public ViewHUD viewHUD;
    public ViewInventory viewInventory;
    public SellInventory sellInventory;
    public BuyInventory buyInventory;

    public bool AnyInventoryShown = false;

    private Dictionary<EffectLogic, float> activeEffects = new Dictionary<EffectLogic, float>();
    public int Money;

    // Use this for initialization

    void Start()
    {
        // instantiate the presentation object
        Presentation = Instantiate(Template.Presentation, transform);
        TargetMarker = Instantiate(Template.TargetMarker, transform);
        if (Inventory == null)
        {
            Inventory = new Inventory();
        }

        HP = Template.MaxHealth;
        Mana = Template.MaxMana;
        MaxSpeed = Template.MaxSpeed;
        Stamina = Template.Stamina;
        SetTarget(null);
    }

    // Update is called once per frame
    void Update()
    {
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
        Stamina = Mathf.Min(Stamina + Template.StaminaRegeneration * Time.deltaTime, Template.Stamina);
        if (Stamina <= 0)
        {
            StaminaEmpty = true;
        }

        if (StaminaEmpty && Stamina > Template.StaminaMinUsage)
        {
            StaminaEmpty = false;
        }
    }

    public float GetArmorResistence()
    {
        float armorResistence = 1;
        for (int i = 0; i < Inventory.OFFSET_SLOT; i++)
        {
            var item = Inventory.GetObject(i);
            if (item != null)
            {
                var iitem = item.GetComponentInChildren<Item>();
                if (iitem)
                {
                    armorResistence += iitem.ArmorResistence;
                }
            }
        }

        return armorResistence;
    }

    public void Move(float dx, float dy, bool sprint)
    {
        if (IsDead())
            return;
        if (AnyInventoryShown)
            return;
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
            var usage = Template.StaminaUsage / Template.Strength * Time.deltaTime;
            if (!StaminaEmpty)
            {
                dx *= (1 + Random.value * 0.2f);
                dy *= (1 + Random.value * 0.2f);
                Stamina -= usage;
            }
        }

        body.AddForce(new Vector2(dx, dy));
    }

    public void StopMovement()
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        body.velocity = new Vector2(0, 0);
    }

    public void Interact()
    {
        if (IsDead())
            return;
        

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
        if (IsDead())
            return;

        var item = Inventory.GetObject(Inventory.HAND_LEFT_SLOT);
        Attack(item);
    }

    public void AttackRight()
    {
        if (IsDead())
            return;

        var item = Inventory.GetObject(Inventory.HAND_RIGHT_SLOT);
        Attack(item);
    }

    public void AttackLeft(UnitLogic unit)
    {
        if (IsDead())
            return;

        var item = Inventory.GetObject(Inventory.HAND_LEFT_SLOT);
        Attack(unit, item);
    }

    public void AttackRight(UnitLogic unit)
    {
        if (IsDead())
            return;

        var item = Inventory.GetObject(Inventory.HAND_RIGHT_SLOT);
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
        if (IsDead())
            return;

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
            hitUnits.Add(Target);
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

        var resistence = GetArmorResistence();
        if (resistence < 1)
        {
            resistence = 1;
        }

        damage /= resistence;
        HP -= damage;
        if (IsDead())
        {
            Die();
        }

        GetComponentInChildren<ShowDamage>().Show(damage);
    }

    public void Die()
    {
        Presentation = Instantiate(Template.DeathPrefab, transform);
        Debug.LogError("I Should be dead by now");
    }

    public bool SwitchTarget()
    {
        if (IsDead())
            return false;

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

            if (hitUnit.gameObject == this || hitUnit.gameObject == Target)
            {
                continue;
            }

            targets.Add(collider.gameObject);
        }

        if (targets.length > 0)
        {
            int idx = (int) (Random.value * targets.length);
            SetTarget((GameObject) targets[idx]);
            return true;
        }

        return false;
    }

    public float GetWeaponRangeLeft()
    {
        var item = Inventory.GetObject(Inventory.HAND_LEFT_SLOT);
        var weapon = item != null ? item.GetComponent<Weapon>() : null;
        var range = weapon != null ? weapon.Range : Template.HandRange;
        return range;
    }

    public float GetWeaponRangeRight()
    {
        var item = Inventory.GetObject(Inventory.HAND_RIGHT_SLOT);
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
        Target = target;
        if (Target != null)
        {
            TargetMarker.transform.SetParent(Target.transform, false);
            TargetMarker.SetActive(true);
        }
        else
        {
            TargetMarker.SetActive(false);
        }
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

    public void Buy(InventoryItem item)
    {
        if (item == null || item.Template == null)
        {
            return;
        }

        var price = item.Template.BasePrice;
        if (price <= Money)
        {
            item.Quantity -= 1;
            Money -= price;
        }
    }

    public void Sell(int slot)
    {
        var item = Inventory.Items[slot];
        if (item == null || item.Template == null)
        {
            return;
        }

        var price = item.Template.BasePrice * 0.75;
        Inventory.Drop(slot, 1);
    }
    
    public void Sell(int slot, Inventory other)
    {
        var item = Inventory.Items[slot];
        if (item == null || item.Template == null)
        {
            return;
        }
        
        other.Take(Inventory.Items[slot]);

        var price = item.Template.BasePrice * 0.75;
        Inventory.Drop(slot, 1);
    }

    public void ShowInventory()
    {
        if (AnyInventoryShown)
        {
            CloseInventories();
            return;
        }
        
        CloseInventories();
        viewInventory.Show(Inventory);
        AnyInventoryShown = true;
    }

    public void BuyInventory(Inventory iv)
    {
        CloseInventories();

        buyInventory.Show(iv);
        AnyInventoryShown = true;
    }

    public void SellInventory(Inventory iv)
    {
        CloseInventories();

        sellInventory.Show(Inventory, iv);
        AnyInventoryShown = true;
    }

    public void ShowMessage(String message, float duration)
    {
        CloseInventories();

        viewHUD.ShowText(message, duration);
        AnyInventoryShown = true;
    }

    public void CloseInventories()
    {
        AnyInventoryShown = false;
        viewInventory.Hide();
        buyInventory.Hide();
        sellInventory.Hide();
        viewHUD.ShowText("", -1);
    }
}