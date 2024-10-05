using UnityEngine;
using Game;

namespace Entities.Enemies {
    public class Titan : Enemy {
        protected void Update() {
            if (playerTarget) {
                agent.SetDestination(playerTarget.position);
            }
            if (agent.remainingDistance <= attackRange && canAttack) {
                PerformAttack();
                StartCoroutine(AttackCoroutine());
            }
        }

        public override void PerformAttack() {
            // Implement attack logic, e.g., reduce player health
            Debug.Log($"{gameObject.name} attacks the defender for {AttackPower} damage!");
            Defender.Instance.TakeDamage(AttackPower);
        }
    }
}