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
    private Vector3 TargetPosition;
    private bool GoHome = false;

    private float durationStuck;
    public PauseScreen pauseScreen;


    private void Start()
    {
        Self = GetComponentInChildren<UnitLogic>();
        SpawnPosition = new Vector2(transform.position.x, transform.position.y);
        pauseScreen = GameObject.FindGameObjectWithTag("Finish").GetComponent<PauseScreen>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        durationStuck = 0;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other == null)
            return;
        if (pauseScreen && pauseScreen.IsShow)
            return;
        var otherUnit = other.gameObject.GetComponent<UnitLogic>();
        if (otherUnit || !Self)
            return;

        Vector2 direction;
        if ((int) durationStuck % 20 < 10)
        {
            direction = Vector2.Perpendicular(TargetPosition - transform.position);
        }
        else
        {
            direction = Vector2.Perpendicular(transform.position - TargetPosition);
        }

        direction.Normalize();


        Self.Move(direction.x, direction.y, false);
        durationStuck += Time.deltaTime;
    }


    private void OnCollisionExit2D(Collision2D other)
    {
        durationStuck = 0;
    }

    private void Update()
    {
        if (!Self)
        {
            return;
        }

        if (pauseScreen && pauseScreen.IsShow)
            return;

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
            else if (Mathf.Abs(distance) < 1e-6)
            {
                GoHome = false;
                Self.StopMovement();
            }

            if (GoHome)
            {
                TargetPosition = SpawnPosition;
                Move();
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

                if (Target != null)
                {
                    TargetPosition = Target.transform.position;
                    Move();
                }
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

    private void Move()
    {
        Self.Move(TargetPosition.x - transform.position.x, TargetPosition.y - transform.position.y, false);
    }
}