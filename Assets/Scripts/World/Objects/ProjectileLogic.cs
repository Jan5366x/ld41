using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    public GameObject Target;
    public Weapon Weapon;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Target)
        {
            UnitLogic unit = Target.GetComponent<UnitLogic>();
            unit.ReceiveDamage(Weapon.Damage);
            unit.ReceiveEffect(Weapon.Effect, Weapon.EffectDuration);
        }
    }

    private void Update()
    {
        var dir = (Target.transform.position - transform.position).normalized;
        transform.Translate(dir * Weapon.ProjectileSpeed * Time.deltaTime);
    }
}