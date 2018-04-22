using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Array = UnityScript.Lang.Array;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    private UnitLogic Self;
    private UnitLogic Target;
    private Vector3 SpawnPosition;
    private bool GoHome = false;

    private void Start()
    {
        Self = GetComponentInChildren<UnitLogic>();
        SpawnPosition = new Vector2(transform.position.x, transform.position.y);
    }

    private void Update()
    {
        if (!Self)
        {
            return;
        }

        if (Self.Template.IsEnemy && Self.CoolDown < 0)
        {
            float distance = (SpawnPosition - transform.position).magnitude;
            if (Target)
            {
                if (Target.IsDead())
                {
                    Target = null;
                    GoHome = true;
                }
            }

            if (distance > Self.Template.FollowRange)
            {
                Target = null;
                GoHome = true;
            }
            else if (distance < Self.Template.HandRange)
            {
                GoHome = false;
            }

            if (GoHome)
            {
                Self.Move(SpawnPosition.x - transform.position.x, SpawnPosition.y - transform.position.y, true);
            }
            else
            {
                var hits = Physics2D.CircleCastAll(new Vector2(transform.position.x, transform.position.y),
                    Self.Template.Intelligence, new Vector2(0, 0));

                if (Target != null)
                {
                    bool didAttack = AttackPlayerInRange(hits);
                    if (!didAttack)
                    {
                        SetNewTarget(hits, 1000 * Time.deltaTime);
                    }
                }
                else
                {
                    SetNewTarget(hits, 100f);
                }

                Self.Move(Target.transform.position.x - transform.position.x,
                    Target.transform.position.y - transform.position.y, false);
            }
        }
    }

    private void SetNewTarget(RaycastHit2D[] hits, float doubleminded)
    {
        foreach (var hit in hits)
        {
            var player = hit.collider.GetComponentInChildren<PlayerMovement>();
            if (player == null)
            {
                continue;
            }

            float distance = (player.transform.position - transform.position).magnitude;
            float distanceSpawn = (SpawnPosition - transform.position).magnitude;
            if (distance < Self.Template.FollowRange && distanceSpawn < Self.Template.FollowRange &&
                Random.value < Self.Template.Intelligence / doubleminded)
            {
                Target = hit.collider.GetComponentInChildren<UnitLogic>();
                return;
            }
        }
    }

    private bool AttackPlayerInRange(RaycastHit2D[] players)
    {
        bool didAttack = false;
        //Attack a random player in range
        foreach (var hit in players)
        {
            var player = hit.collider.GetComponentInChildren<PlayerMovement>();
            var playerUnit = hit.collider.GetComponentInChildren<UnitLogic>();
            if (player == null || playerUnit == null)
            {
                continue;
            }

            float distance = (player.transform.position - transform.position).magnitude;
            if (distance < Self.GetWeaponRangeLeft())
            {
                Self.AttackLeft(playerUnit);
                didAttack = true;
            }
            else if (distance < Self.GetWeaponRangeRight())
            {
                Self.AttackRight(playerUnit);
                didAttack = true;
            }
        }

        return didAttack;
    }
}