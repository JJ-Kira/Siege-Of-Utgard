using UnityEngine;
using UnityEngine.AI;
using Game;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Enemies {
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Enemy : MonoBehaviour {
        [Header("Enemy Stats")] 
        public int MaxHealth = 50;
        protected int currentHealth;
        
        public int AttackPower = 10;

        [Header("Pathfinding")] 
        public float DetectionRange = 10f; // Range within which enemies detect the player
        public float AttackRange = 2f; // Range within which enemies attack the player
        
        protected NavMeshAgent agent;
        protected ThirdPersonCharacter character;
        public Transform castleTarget; // The castle the enemies are moving towards
        public Transform playerTarget; // The player the enemies can attack if nearby
        private bool chasingPlayer = false; // Track whether the enemy is chasing the player
        
        protected virtual void Awake() {
            agent = GetComponent<NavMeshAgent>();
            //character = GetComponent<ThirdPersonCharacter>();
        }

        protected virtual void Start() {
            currentHealth = MaxHealth;
            //animator.updateRotation = false;
        }

        protected virtual void Update()
        {
            //if (agent.remainingDistance > agent.stoppingDistance)
            //    character.Move(agent.desiredVelocity, false, false);
            //else
            //     character.Move(Vector3.zero, false, false);
            
            // Check if the player is within detection range
            float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.position);

            if (distanceToPlayer <= DetectionRange) {
                // If the player is in range and the enemy is not already chasing them
                if (!chasingPlayer) {
                    chasingPlayer = true;
                    SetDestination(playerTarget); // Start chasing the player
                }

                // If the player is within attack range, stop and attack
                if (distanceToPlayer <= AttackRange) {
                    AttackPlayer();
                }
                else {
                    ResumeMovement();
                }
            }
            else {
                // If the player is out of range and the enemy was chasing them, return to the castle
                if (chasingPlayer)
                {
                    ResumeMovement();
                    chasingPlayer = false;
                    SetDestination(castleTarget); // Return to the castle path
                }
            }
        }

        public void TakeDamage(int amount) {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
            // Add UI update/death logic here
            if (currentHealth <= 0) {
                Die();
            }
        }

        protected virtual void Die() {
            WaveManager waveManager = FindObjectOfType<WaveManager>();
            if (waveManager != null) {
                waveManager.OnEnemyDestroyed();
            }

            Debug.Log($"{gameObject.name} has died!");
            Destroy(gameObject);
        }

        // Method to set the target dynamically when instantiating the enemy

        public void SetTargets(bool stormCastle, Transform defender, Transform castle) {
            playerTarget = defender;
            castleTarget = castle;
            SetDestination(stormCastle ? castle : defender);
        }

        protected void SetDestination(Transform newTarget) {
            if (newTarget) {
                agent.SetDestination(newTarget.position);
            }
        }

        // Abstract method for enemy-specific behavior
        public abstract void PerformAttack();
        
        // Method to handle attacking the player (can be expanded)
        private void AttackPlayer()
        {
            // Example attack logic
            Debug.Log("Attacking the player!");
            // You could stop the agent movement here or apply damage to the player
            agent.isStopped = true; // Stop movement to attack
        }

        // Method to resume movement after an attack (if needed)
        public void ResumeMovement()
        {
            agent.isStopped = false;
        }
    }
}