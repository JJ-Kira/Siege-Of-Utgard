using UnityEngine;
using Game;

namespace Entities.Enemies {
    public class Zombie : Enemy {
        private bool chasingPlayer = false; // Track whether the enemy is chasing the player
        private void Update() {
            // Check if the player is within detection range
            float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

            if (distanceToPlayer <= DetectionRange) {
                // If the player is in range and the enemy is not already chasing them
                if (!chasingPlayer) {
                    chasingPlayer = true;
                    SetDestination(playerTarget); // Start chasing the player
                    Debug.LogWarning("Start chasing player");
                }

                // If the player is within attack range, attack
                if (distanceToPlayer <= attackRange && canAttack)
                {
                    PerformAttack();
                    StartCoroutine(AttackCoroutine());
                    Debug.LogWarning("Attacking player");
                }
                else {
                    ResumeMovement();
                    Debug.LogWarning("Resuming movement");
                }
            }
            else {
                // If the player is out of range and the enemy was chasing them, return to the castle
                if (chasingPlayer) {
                    ResumeMovement();
                    chasingPlayer = false;
                    SetDestination(castleTarget); // Return to the castle path
                    Debug.LogWarning("Returning to storming the castle");
                }
                if (castleTarget) {
                    agent.SetDestination(castleTarget.position);
                    Debug.LogWarning("Storming the csatle");
                }
            }
        }

        public override void PerformAttack() {
            // Implement attack logic, e.g., reduce player health
            //animator.SetBool("Attack", true);
            agent.isStopped = true; // Stop movement to attack
            transform.LookAt(chasingPlayer ? playerTarget : door);
            animator.SetTrigger("AttackNow");
            string target = chasingPlayer ? "player" : "cabin";
            Debug.Log($"{gameObject.name} attacks the {target} for {AttackPower} damage!");
            Defender.Instance.TakeDamage(AttackPower);
        }
    }
}