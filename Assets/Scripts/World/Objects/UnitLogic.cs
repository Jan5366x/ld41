using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityScript.Lang;

public class UnitLogic : MonoBehaviour
{
    public Unit Template;

    public float HP;
    public float Mana;

    private GameObject _presentation;
    private GameObject _target;
    private GameObject _targetMarker;
    public Inventory _inventory;

    // Use this for initialization

    void Start()
    {
        // instantiate the presentation object
        _presentation = Instantiate(Template.Presentation, transform);
        _targetMarker = Instantiate(Template.TargetMarker, transform);
        _inventory = gameObject.AddComponent<Inventory>();

        HP = Template.MaxHealth;
        Mana = Template.MaxMana;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public GameObject Presentation
    {
        get { return _presentation; }
    }

    public void move(float dx, float dy, bool sprint)
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
        var hits = Physics2D.CircleCastAll(new Vector2(transform.position.x, transform.position.y), 0.5f,
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
    }

    public void AttackA()
    {
        Debug.Log("++++," + _inventory.ToString());
        var item = _inventory.GetObject(Inventory.HAND_LEFT_SLOT);
        Debug.Log("----,", item);
        Attack(item);
    }

    public void AttackB()
    {
        var item = _inventory.GetObject(Inventory.HAND_RIGHT_SLOT);
        Attack(item);
    }

    private void Attack(GameObject weapon)
    {
        var damageScript = weapon == null ? null : weapon.GetComponent<Weapon>();
        var damage = damageScript == null ? 1 : damageScript.Damage;
        var units = getHitUnits(damageScript);
        foreach (var unit in units)
        {
            unit.ReceiveDamage(damage);
        }
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
        HP -= damage;
        if (HP < 0)
        {
            Die();
        }
        
        print(GetComponentInChildren<ShowDamage>());

        GetComponentInChildren<ShowDamage>().Show(damage);
    }

    public void Die()
    {
        _presentation = Instantiate(Template.DieSprite, transform);
        Debug.LogError("I Should be dead by now");
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
        _targetMarker.transform.SetParent(_target.transform);
    }
}