using UnityEngine;
using World.Objects;

public class ProjectileLogic : MonoBehaviour
{
    private GameObject _target;
    private Weapon _weapon;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == _target)
        {
            onHit();
        }
    }

    private void Update()
    {
        if (!_target || !_weapon)
            return;
        var delta = _target.transform.position - transform.position;
        var dir = delta.normalized;
        if (delta.sqrMagnitude < _weapon.ProjectileSpeed * Time.deltaTime)
        {
            transform.Translate(delta);
            onHit();
        }
        else
        {
            transform.Translate(dir * _weapon.ProjectileSpeed * Time.deltaTime);
        }
    }

    public void Fire(UnitLogic target, Weapon weapon)
    {
        _target = target.gameObject;
        _weapon = weapon;
    }

    private void onHit()
    {
        if (_target && _weapon)
        {
            UnitLogic unit = _target.GetComponent<UnitLogic>();
            unit.ReceiveDamage(_weapon.Damage);
            unit.ReceiveEffect(_weapon.Effect, _weapon.EffectDuration);
        }

        Destroy(gameObject);
    }
}