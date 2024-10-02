using UnityEngine;

namespace Game {
    public class Defender : MonoBehaviour {
        // Singleton instance for easy access
        public static Defender Instance { get; private set; }

        [Header("Player Stats")] public int MaxHealth = 100;
        private int currentHealth;

        [Header("Resources")] public int StartingResources = 100;
        private int currentResources;

        private void Awake() {
            // Implement Singleton pattern
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Persist across scenes if necessary
            }
            else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            currentHealth = MaxHealth;
            currentResources = StartingResources;
        }

        #region Health Management

        public void TakeDamage(int amount) {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
            // Add UI update or death logic here
            if (currentHealth <= 0) {
                Die();
            }
        }

        public void Heal(int amount) {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
            // Add UI update here
        }

        private void Die() {
            // Handle player death (e.g., game over screen)
            Debug.Log("Player has died!");
        }

        #endregion

        #region Resource Management

        public bool SpendResources(int amount) {
            if (currentResources >= amount) {
                currentResources -= amount;
                // Update UI here
                return true;
            }

            return false;
        }

        public void AddResources(int amount) {
            currentResources += amount;
            // Update UI here
        }

        public int GetCurrentResources() {
            return currentResources;
        }

        #endregion
    }
}