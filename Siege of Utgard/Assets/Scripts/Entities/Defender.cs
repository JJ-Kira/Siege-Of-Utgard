using UnityEngine;

namespace Game {
    public class Defender : Entity {
        // Singleton instance for easy access
        public static Defender Instance { get; private set; }

        public int StartingResources = 100;
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

        protected override void Start()
        {
            base.Start();
            currentResources = StartingResources;
        }
        
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

        protected override void Die()
        {
            base.Die();
        }
    }
}