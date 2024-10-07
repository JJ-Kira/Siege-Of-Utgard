using UnityEngine;
using Game;

namespace Entities.Enemies {
    public class Zombie : Enemy {
        private static readonly int Attack = Animator.StringToHash("Attack");
        
        private bool chasingPlayer = false; // Track whether the enemy is chasing the player
        private bool stormingCastle = false;
        
        private void Update() {
            // Check if the player is within detection range
            var distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);
            var distanceToCabin = Vector3.Distance(transform.position, castleTarget.position);
            
            if (stormingCastle && canAttack) {
                transform.rotation = Quaternion.LookRotation(new Vector3(cabinCenter.transform.position.x, transform.position.y, 
                    cabinCenter.transform.position.z) - transform.position, Vector3.up);
                isAttacking = true;
                PerformAttack();
                StartCoroutine(AttackCoroutine());
            }
            else if (distanceToPlayer <= DetectionRange) {
                // If the player is in range and the enemy is not already chasing them
                if (!chasingPlayer) {
                    chasingPlayer = true;
                    SetDestination(playerTarget); // Start chasing the player
                }

                // If the player is within attack range, attack
                if (distanceToPlayer <= attackRange && canAttack) {
                    transform.rotation = Quaternion.LookRotation(new Vector3(playerTarget.transform.position.x, transform.position.y, 
                        playerTarget.transform.position.z) - transform.position, Vector3.up);
                    isAttacking = true;
                    PerformAttack();
                    StartCoroutine(AttackCoroutine());
                }
                else if (distanceToPlayer > attackRange) {
                    ResumeMovement();
                }
            }
            else {
                // If the player is out of range and the enemy was chasing them, return to the castle
                if (chasingPlayer) {
                    ResumeMovement();
                    chasingPlayer = false;
                }
                if (castleTarget) 
                    agent.SetDestination(castleTarget.position);
                if (distanceToCabin <= attackRange) {
                    stormingCastle = true;
                    agent.isStopped = true;
                    //transform.LookAt(door);
                }
            }
        }

        public override void PerformAttack() {
            agent.isStopped = true; // Stop movement to attack
            //transform.LookAt(chasingPlayer ? playerTarget : door);
            animator.SetTrigger(Attack);
            string target = chasingPlayer ? "player" : "cabin";
            Debug.Log($"{gameObject.name} attacks the {target} for {AttackPower} damage!");
            if (chasingPlayer)
                Defender.Instance.TakeDamage(AttackPower);
            else
                Castle.Instance.TakeDamage(AttackPower);
        }
    }
}