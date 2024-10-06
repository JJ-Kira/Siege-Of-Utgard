using System.Runtime.CompilerServices;
using UnityEngine;
using Game;

namespace Entities.Enemies {
    public class Zombie : Enemy {
        private static readonly int Attack = Animator.StringToHash("Attack");
        private bool chasingPlayer = false; // Track whether the enemy is chasing the player
        
        private void Update() {
            // Check if the player is within detection range
            var distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

            if (distanceToPlayer <= DetectionRange) {
                // If the player is in range and the enemy is not already chasing them
                if (!chasingPlayer) {
                    chasingPlayer = true;
                    SetDestination(playerTarget); // Start chasing the player
                }

                // If the player is within attack range, attack
                if (distanceToPlayer <= attackRange && canAttack) {
                    isAttacking = true;
                    PerformAttack();
                    StartCoroutine(AttackCoroutine());
                }
                else {
                    ResumeMovement();
                }
            }
            else {
                // If the player is out of range and the enemy was chasing them, return to the castle
                if (chasingPlayer) {
                    ResumeMovement();
                    chasingPlayer = false;
                    SetDestination(castleTarget); // Return to the castle path
                }
                if (castleTarget) {
                    agent.SetDestination(castleTarget.position);
                    
                    var distanceToCabin = Vector3.Distance(transform.position, castleTarget.position);
                    if (distanceToCabin <= DetectionRange && canAttack) {
                        isAttacking = true;
                        PerformAttack();
                        StartCoroutine(AttackCoroutine());
                    }
                    else {
                        ResumeMovement();
                    }
                }
            }
        }

        public override void PerformAttack() {
            agent.isStopped = true; // Stop movement to attack
            transform.LookAt(chasingPlayer ? playerTarget : door);
            animator.SetTrigger(Attack);
            Defender.Instance.TakeDamage(AttackPower);
            string target = chasingPlayer ? "player" : "cabin";
            Debug.Log($"{gameObject.name} attacks the {target} for {AttackPower} damage!");
            if (chasingPlayer)
                Defender.Instance.TakeDamage(AttackPower);
            else
                Castle.Instance.TakeDamage(AttackPower);
        }
    }
}