using UnityEngine;

namespace Game {
    public class SiegeManager : MonoBehaviour {
        [HideInInspector] public static SiegeManager Instance { get; private set; }

        [Header("Wave Settings")]
        public WaveManager WaveManager;

        [Header("Game State")]
        public bool IsGameOver = false;

        private void Awake() {
            // Implement Singleton pattern
            if (Instance == null) {
                Instance = this;
                // Optionally, use DontDestroyOnLoad if needed
            }
            else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            if (WaveManager != null) {
                WaveManager.StartWave();
            }
        }

        public void GameOver() {
            IsGameOver = true;
            // Handle game over logic (e.g., display UI, stop wave spawning)
            Debug.Log("Game Over!");
        }

        public void Victory() {
            // Handle victory logic (e.g., display UI, stop wave spawning)
            Debug.Log("You Win!");
        }
    }
}