using UI;
using UnityEngine;

namespace World.Objects
{
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
                if (Target)
                {
                    if (Target.IsDead())
                    {
                        Target = null;
                        GoHome = true;
                    }
                }

                float distanceFromHome = Distance2D.getDistance(SpawnPosition, gameObject);
                if (distanceFromHome > Self.Template.FollowRange)
                {
                    Target = null;
                    GoHome = true;
                }
                else if (Mathf.Abs(distanceFromHome) < Self.Template.HandRange)
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
                    var hits = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y),
                        Self.Template.Intelligence);

                    if (Target != null)
                    {
                        AttackPlayerInRange(hits);
                    }
                    else
                    {
                        SetNewTarget(hits);
                    }

                    if (Target != null)
                    {
                        TargetPosition = Target.transform.position;
                        Move();
                    }
                }
            }
        }

        private void SetNewTarget(Collider2D[] hits)
        {
            foreach (var hit in hits)
            {
                var player = hit.GetComponentInChildren<PlayerMovement>();
                if (player == null)
                {
                    continue;
                }

                float distance = (player.transform.position - transform.position).magnitude;
                float distanceSpawn = (SpawnPosition - transform.position).magnitude;
                if (distance < Self.Template.FollowRange && distanceSpawn < Self.Template.FollowRange)
                {
                    Target = hit.GetComponentInChildren<UnitLogic>();
                    return;
                }
            }
        }

        private bool AttackPlayerInRange(Collider2D[] players)
        {
            bool didAttack = false;
            //Attack a random player in range
            foreach (var hit in players)
            {
                var player = hit.GetComponentInChildren<PlayerMovement>();
                var playerUnit = hit.GetComponentInChildren<UnitLogic>();
                if (player == null || playerUnit == null)
                {
                    continue;
                }

                float distance = Distance2D.getDistance(player.gameObject, gameObject);
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
}