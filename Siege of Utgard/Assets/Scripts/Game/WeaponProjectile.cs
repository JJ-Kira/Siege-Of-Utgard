using Entities.Enemies;
using UnityEngine;

namespace Game {
    public class WeaponProjectile : MonoBehaviour {
        [SerializeField] private float lifetime = 3f; // Time before the projectile self-destructs
        [SerializeField] private int damage = 10;     // Amount of damage the projectile will deal to enemies

        private void Start() {
            Destroy(gameObject, lifetime);
        }

        private void OnTriggerEnter(Collider other) {
            // Check if the object we collided with is an enemy
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null) {
                // Call TakeDamage on the enemy and pass the damage value
                Defender.Instance.DealDamage(damage, enemy);

                // Destroy the projectile after it hits the enemy
                Destroy(gameObject);
            }
        }
    }
}