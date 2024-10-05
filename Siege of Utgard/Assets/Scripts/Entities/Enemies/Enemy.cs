using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Game;

namespace Entities.Enemies {
    [RequireComponent(typeof(NavMeshAgent), (typeof(Animator)))]
    public abstract class Enemy : Entity {
        private static readonly int Death = Animator.StringToHash("Die");

        [Header("Enemy Stats")] 
        protected float AttackCooldown = 2f;
        public float AttackPower = 10;

        [Header("Pathfinding")] 
        public float DetectionRange = 5f; // Range within which enemies detect the player
        protected float attackRange; // Range within which enemies attack the player
        
        protected NavMeshAgent agent;
        public Transform castleTarget, playerTarget, door;

        protected Animator animator;
        protected bool canAttack = true;
        protected virtual void Awake() {
            agent = GetComponent<NavMeshAgent>(); //TODO: Handle both by WaveManager
            animator = GetComponent<Animator>();

            attackRange = agent.stoppingDistance + 0.5f;
        }

        protected override void Die() {
            base.Die();
            WaveManager waveManager = FindObjectOfType<WaveManager>();
            if (waveManager) {
                waveManager.OnEnemyDestroyed();
            }
            animator.SetBool(Death, true);
            Destroy(gameObject, 5f);
        }

        // Method to set the target dynamically when instantiating the enemy

        public void SetTargets(bool stormCastle, Transform defender, Transform castle, Transform door) {
            playerTarget = defender;
            castleTarget = castle;
            this.door = door;
            SetDestination(stormCastle ? castle : defender);
        }

        protected void SetDestination(Transform newTarget) {
            if (newTarget) {
                agent.SetDestination(newTarget.position);
            }
        }

        // Abstract method for enemy-specific behavior
        public abstract void PerformAttack();

        // Method to resume movement after an attack (if needed)
        protected void ResumeMovement() {
            agent.isStopped = false;
        }
        
        // Coroutine to handle attack logic with cooldown
        protected IEnumerator AttackCoroutine()
        {
            agent.isStopped = true;  // Stop moving to attack the player
            canAttack = false;       // Set canAttack to false to prevent continuous attacks

            // Attack logic (e.g., damage the player)
            // Example: Deal damage to player here

            yield return new WaitForSeconds(AttackCooldown);  // Wait for the cooldown duration

            // Resume movement after cooldown
            agent.isStopped = false; // Resume movement
            canAttack = true;        // Allow attacking again
        }
    }
}