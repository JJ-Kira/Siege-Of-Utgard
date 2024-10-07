using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Game;

namespace Entities.Enemies {
    [RequireComponent(typeof(NavMeshAgent), (typeof(Animator)))]
    public abstract class Enemy : Entity {
        private static readonly int Death = Animator.StringToHash("Die");

        [Header("Enemy Stats")] 
        public float AttackCooldown = 2f;
        public float AttackPower = 10;
        public float ExperienceValue;

        [Header("Pathfinding")] 
        public float DetectionRange = 5f; // Range within which enemies detect the player
        protected float attackRange; // Range within which enemies attack the player
        
        protected NavMeshAgent agent;
        protected Transform playerTarget, castleTarget, cabinCenter;

        protected Animator animator;
        protected bool canAttack = true;
        protected bool isAttacking = false;
        protected bool isDying = false;
        
        protected virtual void Awake() {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            attackRange = agent.stoppingDistance + 0.1f;
        }

        private void Start()
        {
            base.Start();
        }
        
        protected override void Die() {
            base.Die();
            isDying = true;
            Defender.Instance.GainExperience(ExperienceValue);
            WaveManager waveManager = FindObjectOfType<WaveManager>();
            if (waveManager) {
                waveManager.OnEnemyDestroyed();
            }
            animator.SetTrigger(Death);
            Destroy(gameObject, 5f);
        }

        // Method to set the target dynamically when instantiating the enemy

        public void SetTargets(Transform defender, Transform castle, Transform cabinCenter) {
            playerTarget = defender;
            castleTarget = castle;
            this.cabinCenter = cabinCenter;
        }

        // Abstract method for enemy-specific behavior
        public abstract void PerformAttack();

        // Method to resume movement after an attack (if needed)
        protected void ResumeMovement() {
            agent.isStopped = false;
            if (isAttacking)
            {
                isAttacking = false;
                canAttack = true;
                StopCoroutine(AttackCoroutine());
            }
        }

        protected IEnumerator MoveTowardsTarget(Vector3 target)
        {
            agent.SetDestination(target);

            yield return new WaitForSeconds(3);
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
            //agent.isStopped = false; // Resume movement
            canAttack = true;        // Allow attacking again
        }
    }
}