using System;
using UnityEngine;

namespace Game {
    public class SiegeManager : MonoBehaviour
    {
        public static SiegeManager Instance { get; private set; }

        public WaveManager WaveManager;
        [Min(0)] public int Waves;
        [SerializeField] private float[] wavesExperienceParameters;
        [SerializeField] private int[] wavesEnemyCountParameters;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            if (WaveManager != null)
            {
                WaveManager.StartWave(wavesExperienceParameters[0], wavesEnemyCountParameters[0]);
            }
        }

        public void Victory()
        {
            // Handle victory logic (e.g., display UI, stop wave spawning)
            Debug.Log("You Win!");
            Defender.Instance.AnnounceVictory();
            //TODO: next wave & buff shops from experience point
        }

        void OnValidate()
        {
            if (wavesExperienceParameters == null || wavesExperienceParameters.Length == 0)
                Array.Resize(ref wavesExperienceParameters, Waves);
            if (wavesEnemyCountParameters == null || wavesEnemyCountParameters.Length == 0)
                Array.Resize(ref wavesEnemyCountParameters, Waves);
        }
    }
}