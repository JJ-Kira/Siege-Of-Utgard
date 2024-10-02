using UnityEngine;
using UnityEngine.AI;
using Game;

namespace Enemies {
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Enemy : MonoBehaviour {
        [Header("Enemy Stats")] public int MaxHealth = 50;
        protected int currentHealth;

        public int AttackPower = 10;
        public float speed = 3.5f;

        [Header("Pathfinding")] protected NavMeshAgent agent;
        private Transform target; // Typically, the player's base or a specific point

        protected virtual void Awake() {
            agent = GetComponent<NavMeshAgent>();
        }

        protected virtual void Start() {
            currentHealth = MaxHealth;
            SetDestination();
        }

        protected virtual void Update() {
            // Add common behaviors here
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
        public void SetTarget(Transform newTarget) {
            target = newTarget;
            SetDestination();
        }

        protected void SetDestination() {
            if (target != null) {
                agent.SetDestination(target.position);
            }
        }

        // Abstract method for enemy-specific behavior
        public abstract void PerformAttack();
    }
}