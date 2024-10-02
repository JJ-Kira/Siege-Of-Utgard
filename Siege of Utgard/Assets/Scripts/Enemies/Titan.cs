using UnityEngine;
using Game;

namespace Enemies {
    public class Titan : Enemy {
        [Header("Basic Enemy Specifics")] public float AttackRange = 1.5f;
        public float AttackCooldown = 2f;
        private float lastAttackTime;

        protected override void Start() {
            base.Start();
            // Additional initialization if needed
        }

        protected override void Update() {
            base.Update();
            HandleMovementAndAttack();
        }

        private void HandleMovementAndAttack() {
            if (agent.remainingDistance <= AttackRange && Time.time >= lastAttackTime + AttackCooldown) {
                PerformAttack();
                lastAttackTime = Time.time;
            }
        }

        public override void PerformAttack() {
            // Implement attack logic, e.g., reduce player health
            Debug.Log($"{gameObject.name} attacks the player for {AttackPower} damage!");
            Defender.Instance.TakeDamage(AttackPower);
        }

        protected override void Die() {
            base.Die();
            // Additional death logic (e.g., spawn particles)
        }
    }
}