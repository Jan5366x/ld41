﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Effects;
using TemplateSkeleton;
using UI;
using UnityEngine;
using World.Interactables;
using Array = UnityScript.Lang.Array;
using Random = UnityEngine.Random;

namespace World.Objects
{
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

        public const float AnimationFrameTime = 0.3f;
        public float RemainingAnimationTime;
        public SpriteAnimator baseAnimator;
        public SpriteAnimator attachmentAnimator;
        public SpriteAnimator headAnimator;
        public SpriteAnimator armorAnimator;
        public SpriteAnimator legsAnimator;
        public SpriteAnimator bootsAnimator;
        public SpriteAnimator handAAnimator;
        public SpriteAnimator handBAnimator;


        public ViewHUD viewHUD;
        public ViewInventory viewInventory;
        public SellInventory sellInventory;
        public BuyInventory buyInventory;

        public bool AnyInventoryShown = false;

        public int Money;
        public PauseScreen pauseScreen;

        private bool standingStill = true;

        private List<SpriteAnimator> animationHelper;

        // Use this for initialization

        public enum MoveDirection
        {
            Up = 0,
            Down = 1,
            Left = 2,
            Right = 3
        }

        void Start()
        {
            if (Template.IsPlayer)
            {
                Inventory = new Inventory();
            }

            // instantiate the presentation object
            Presentation = Instantiate(Template.Presentation, transform);
            if (Template.TargetMarker)
            {
                TargetMarker = Instantiate(Template.TargetMarker, transform);
            }

            HP = Template.MaxHealth;
            Mana = Template.MaxMana;
            MaxSpeed = Template.MaxSpeed;
            Stamina = Template.Stamina;
            SetTarget(null);

            try
            {
                pauseScreen = GameObject.FindGameObjectWithTag("Finish").GetComponent<PauseScreen>();
                SetupAnimationHelper();
                UpdatePresentation();
            }
            catch (NullReferenceException ex)
            {
                print("FUZUUUUUUUUUUUUU");
            }
        }

        private void SetupAnimationHelper()
        {
            baseAnimator = Presentation.GetComponent<SpriteAnimator>();
            attachmentAnimator = Presentation.transform.Find("Hair").GetComponent<SpriteAnimator>();
            headAnimator = Presentation.transform.Find("Head").GetComponent<SpriteAnimator>();
            armorAnimator = Presentation.transform.Find("Body").GetComponent<SpriteAnimator>();
            legsAnimator = Presentation.transform.Find("Legs").GetComponent<SpriteAnimator>();
            bootsAnimator = Presentation.transform.Find("Boots").GetComponent<SpriteAnimator>();
            handAAnimator = Presentation.transform.Find("HandA").GetComponent<SpriteAnimator>();
            handBAnimator = Presentation.transform.Find("HandB").GetComponent<SpriteAnimator>();

            animationHelper = new List<SpriteAnimator>
            {
                baseAnimator,
                attachmentAnimator,
                headAnimator,
                armorAnimator,
                legsAnimator,
                bootsAnimator,
                handAAnimator,
                handBAnimator
            };
        }

        // Update is called once per frame
        void Update()
        {
            if (pauseScreen && pauseScreen.IsShow)
            {
                return;
            }

            if (IsDead())
            {
                if (Template.IsEnemy)
                {
                    Destroy(gameObject);
                }

                return;
            }

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

            if (Inventory != null && Inventory.PresentationChanged)
            {
                UpdatePresentation();
                Inventory.PresentationChanged = false;
            }

            if (RemainingAnimationTime < 0)
            {
                UpdateAnimation();
                RemainingAnimationTime = AnimationFrameTime;
            }

            RemainingAnimationTime -= Time.deltaTime;
        }

        private void UpdatePresentation()
        {
            UpdateAnimator(baseAnimator, Template.GetBodySprites());
            UpdateAnimator(attachmentAnimator, Template.GetAttachmentSprites());
            UpdateAnimator(headAnimator, (int) ItemSlot.Head);
            UpdateAnimator(armorAnimator, (int) ItemSlot.Armor);
            UpdateAnimator(legsAnimator, (int) ItemSlot.Pants);
            UpdateAnimator(bootsAnimator, (int) ItemSlot.Shoes);
            UpdateAnimator(handAAnimator, (int) ItemSlot.LeftHand);
            UpdateAnimator(handBAnimator, (int) ItemSlot.RightHand);
        }

        private void UpdateAnimator(SpriteAnimator animator, int slot)
        {
            UpdateAnimator(animator, Inventory.GetItem(slot));
        }

        private void UpdateAnimator(SpriteAnimator animator, Item item)
        {
            if (animator == null)
                return;
            animator.SetByItem(item);
        }

        private void UpdateAnimator(SpriteAnimator animator, Sprite[] sprites)
        {
            if (animator == null)
                return;
            animator.SetSprites(sprites);
        }

        private void UpdateAnimationDirection(MoveDirection direction)
        {
            if (animationHelper == null)
                return;

            foreach (var animator in animationHelper)
            {
                animator.SetDirection(direction);
            }
        }

        private void UpdateAnimation()
        {
            if (animationHelper == null)
                return;

            foreach (SpriteAnimator animator in animationHelper)
            {
                if (!animator)
                    continue;

                if (standingStill)
                {
                    animator.Idle();
                }
                else
                {
                    animator.NextSprite();
                }
            }
        }

        public float GetArmorResistence()
        {
            float armorResistence = 0;
            if (Inventory == null)
                return armorResistence;
            for (int i = 0; i < Inventory.OFFSET_SLOT; i++)
            {
                var item = Inventory.GetItem(i);
                if (item != null)
                {
                    armorResistence += item.ArmorResistence;
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
                if (dx < 0)
                {
                    UpdateAnimationDirection(MoveDirection.Left);
                }
                else
                {
                    UpdateAnimationDirection(MoveDirection.Right);
                }

                dy = 0;
            }
            else if (Mathf.Abs(dx) < Mathf.Abs(dy))
            {
                dx = 0;
                if (dy < 0)
                {
                    UpdateAnimationDirection(MoveDirection.Up);
                }
                else
                {
                    UpdateAnimationDirection(MoveDirection.Down);
                }
            }

            dx *= Template.Acceleration;
            dy *= Template.Acceleration;

            if (sprint)
            {
                var usage = Template.StaminaUsage / Template.Strength * Time.deltaTime;
                if (!StaminaEmpty)
                {
                    dx *= 1.5f;
                    dy *= 1.5f;
                    Stamina -= usage;
                }
            }

            Vector2 forceVec = new Vector2(dx, dy);
            body.AddForce(forceVec);

            standingStill = false;
        }


        public void StopMovement()
        {
            Rigidbody2D body = GetComponent<Rigidbody2D>();
            body.velocity = new Vector2(0, 0);
            standingStill = true;
        }

        public void Interact()
        {
            if (IsDead())
                return;

            var hits = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y),
                10 * Template.HandRange);

            int minIdx = -1;
            float minDist = 99999;
            int idx = 0;
            Interactable bestInteractable = null;

            foreach (var hit in hits)
            {
                var collider = hit.gameObject;
                var interactable = collider.GetComponentInChildren<Interactable>();
                float dx = transform.position.x - collider.transform.position.x;
                float dy = transform.position.y - collider.transform.position.y;
                float d = Mathf.Sqrt(dx * dx + dy * dy);

                if (d < minDist && interactable != null)
                {
                    minDist = d;
                    minIdx = idx;
                    bestInteractable = interactable;
                }

                idx++;
            }

            if (minIdx >= 0 && bestInteractable != null)
            {
                Debug.DrawLine(gameObject.transform.position, bestInteractable.transform.position, Color.cyan);
                if (bestInteractable.CanInteract(this))
                {
                    bestInteractable.Interact(this);
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

            var item = Inventory != null ? Inventory.GetObject(ItemSlot.LeftHand) : null;
            Attack(item);
        }

        public void AttackRight()
        {
            if (IsDead())
                return;

            var item = Inventory != null ? Inventory.GetObject(ItemSlot.RightHand) : null;
            Attack(item);
        }

        public void AttackLeft(UnitLogic unit)
        {
            if (IsDead())
                return;

            var item = Inventory != null ? Inventory.GetObject(ItemSlot.LeftHand) : null;
            Attack(unit, item);
        }

        public void AttackRight(UnitLogic unit)
        {
            if (IsDead())
                return;

            var item = Inventory != null ? Inventory.GetObject(ItemSlot.RightHand) : null;
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
            if (target && target != this)
            {
                float damage = weapon != null ? weapon.Damage : Template.Strength;
                EffectLogic
                    effect = weapon != null ? weapon.Effect : null; //target.gameObject.AddComponent<PoisonEffectLogic>();
                float effectDuration = weapon != null ? weapon.EffectDuration : 2;
                target.ReceiveDamage(damage);
                target.ReceiveEffect(effect, effectDuration);
            }
        }

        private void FireProjectile(Weapon weapon, UnitLogic target)
        {
            if (weapon == null || target == null)
                return;

            SetTarget(new[] {target.gameObject});
        
            if (Mana < weapon.ManaUsage)
            {
                return;
            }

            var projectile = Instantiate(weapon.Projectile, transform);
            projectile.transform.SetParent(null, true);
            var projectileScript = projectile.GetComponentInChildren<ProjectileLogic>();
            if (projectileScript)
            {
                projectileScript.Fire(target, weapon);
                Mana -= weapon.ManaUsage;
            }
        }

        private UnitLogic[] getHitUnits(Weapon weapon)
        {
            Array hitUnits = new Array();
            if (weapon != null && weapon.Magic && Target != null)
            {
                var unitLogic = Target.GetComponent<UnitLogic>();
                hitUnits.Add(unitLogic);
            }
            else
            {
                var range = weapon != null ? weapon.Range : Template.HandRange;
                var multipleHits = weapon != null ? weapon.Multi : false;
                var hits = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), range);

                float minDist = 99999;
                UnitLogic nearestUnit = null;
                foreach (var hit in hits)
                {
                    var collider = hit.gameObject;
                    var hitUnit = collider.GetComponentInChildren<UnitLogic>();
                    float dist = (collider.transform.position - transform.position).sqrMagnitude;

                    if (!hitUnit)
                        continue;
              

                    if (!hitUnit.Template.IsEnemy)
                        continue;
            

                    if (Random.value < hitUnit.Template.Agility / 100f)
                        continue;
           

                    if (hitUnit == this)
                        continue;

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

                if (!multipleHits && nearestUnit)
                {
                    hitUnits.Add(nearestUnit);
                    SetTarget(new[] {nearestUnit.gameObject});
                }
            }

            return hitUnits.Cast<UnitLogic>().ToArray();
        }

        public void ReceiveDamage(float damage)
        {
            if (IsDead())
                return;

            var resistence = GetArmorResistence();
            if (resistence < 0)
                resistence = 0;

            damage = Mathf.Max(0, damage - resistence);
            HP -= damage;
            if (IsDead())
            {
                Die();
            }

            GetComponentInChildren<ShowDamage>().Show(damage);
            if (!Mathf.Approximately(damage, 0))
            {
                var blood = gameObject.AddComponent<BloodEffectLogic>();
                blood.apply(this, 1);
            }
        }

        public void Die()
        {
            Debug.LogError("I Should be dead by now");

            if (Template.DeathPrefab)
            {
                Presentation = Instantiate(Template.DeathPrefab, transform);
                Presentation.transform.SetParent(null, true);
                gameObject.SetActive(false);
                return;
            }

            if (Template.DeathDrop)
            {
                Instantiate(Template.DeathDrop);
            }
        }

        public bool SwitchTarget()
        {
            if (IsDead())
                return false;

            var range = GetMaxWeaponRange();
            var unitsInRange =
                Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), range * 1.5f);
            Array targets = new Array();


            for (int i = 0; i < unitsInRange.Length; i++)
            {
                var collider = unitsInRange[i].gameObject;
                var hitUnit = collider.GetComponentInChildren<UnitLogic>();
                if (!hitUnit)
                    continue;

                if (Template.IsPlayer ^ hitUnit.Template.IsEnemy)
                    continue;

                if (hitUnit.gameObject == this || hitUnit.gameObject == Target)
                    continue;

                targets.Add(collider.gameObject);
            }

            if (targets.length > 0)
            {
                SetTarget(targets.Cast<GameObject>().ToArray());
                return true;
            }

            return false;
        }

        public float GetWeaponRangeLeft()
        {
            var item = Inventory != null ? Inventory.GetObject(ItemSlot.LeftHand) : null;
            var weapon = item != null ? item.GetComponent<Weapon>() : null;
            var range = weapon != null ? weapon.Range : Template.HandRange;
            return range;
        }

        public float GetWeaponRangeRight()
        {
            var item = Inventory != null ? Inventory.GetObject(ItemSlot.RightHand) : null;
            var weapon = item != null ? item.GetComponent<Weapon>() : null;
            var range = weapon != null ? weapon.Range : Template.HandRange;
            return range;
        }

        public float GetMaxWeaponRange()
        {
            return Mathf.Max(GetWeaponRangeLeft(), GetWeaponRangeRight());
        }

        public void SetTarget(GameObject[] target)
        {
            if (target != null)
            {
                if (Target)
                {
                    Target = target[(int) (Random.value * target.Length)];
                }
                else
                {
                    GameObject best = null;
                    float bestDist = 99999999;
                    foreach (var t in target)
                    {
                        if (!t)
                            continue;
                        var dist = Distance2D.getDistance(gameObject, t);
                        if (dist < bestDist)
                        {
                            best = t;
                            bestDist = dist;
                        }
                    }

                    Target = best;
                }
            }

            
            // create new target marker if required and possible
            if (!TargetMarker && Template.TargetMarker)
                TargetMarker = Instantiate(Template.TargetMarker, transform);
  
           
            UpdateTargetMarkerActivation();
        }

        
        private void UpdateTargetMarkerActivation()
        {
            if (TargetMarker == null)
                return;
            
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
                weaponEffect.apply(this, weaponEffectDuration);
            }
        }

        public bool IsDead()
        {
            return HP < 0;
        }

        public void Buy(InventoryItem item)
        {
            if (item == null || item.Template == null)
                return;

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
                return;

            var price = item.Template.BasePrice * Item.SellModifier;
            Inventory.Drop(slot, 1);
        }

        public void Sell(int slot, Inventory other)
        {
            var item = Inventory.Items[slot];
            if (item == null || item.Template == null)
                return;

            other.Take(Inventory.Items[slot]);

            var price = item.Template.BasePrice * Item.SellModifier;
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
    
        public void ShowPrefab(string name, float duration)
        {
            var res = Resources.Load(name);
            if (res == null)
                return;

            var obj = Instantiate(res, transform) as GameObject;
            if (obj == null) 
                return;
            var autoDestruct = obj.GetComponentInChildren<AutoDestruct>();
            if (autoDestruct)
            {
                autoDestruct.Fire(obj, duration);
            }
        }

        public void ProxyCoroutine(string name, object[] param)
        {
            StartCoroutine(name, param);
        }
    
        private IEnumerator DealDamageOverTime(object[] param)
        {
            var duration = (float) param[0];
            var tickRate = (float) param[1];
            var damagePerTick = (float) param[2];
        
            var numTicks = (int) (duration / tickRate);
            for (var i = 0; i < numTicks; i++)
            {
                ReceiveDamage(damagePerTick);
                yield return new WaitForSeconds(tickRate);
            }
        }
        private IEnumerator Freeze(object[] param)
        {
            var duration = (float) param[0];
            var oldCooldown = CoolDown;
            CoolDown = 999999;
            MaxSpeed = 0;
            yield return new WaitForSeconds(duration);
            CoolDown = oldCooldown - duration;
            MaxSpeed = Template.MaxSpeed;
        }
    }
}