using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entities.Enemies;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game {
    [System.Serializable]

    public class WaveManager : MonoBehaviour
    {
        [Header("Targets")] 
        [SerializeField] private Transform player;
        [SerializeField] private Transform cabin;
        [SerializeField] private Transform target;

        [Header("Spawning")] 
        public float MaxSpawnDistance;
        public float MinSpawnDistance;
        public GameObject[] EnemyPrefabs;
        public float[] EnemyExperience;
        
        private float maxExperience;
        private int maxEnemies;
        private List<int> waveEnemies = new();
        private int enemyDeathCount = 0;
        
        public void StartWave(float experience, int enemies) {
            // reset the manager
            if (waveEnemies != null && waveEnemies.Count != 0) {
                waveEnemies.Clear();
                enemyDeathCount = 0;
            }
            maxExperience = experience;
            maxEnemies = enemies;
                
            PrepareWave();
            StartCoroutine(SpawnWave());
        }

        private void PrepareWave()
        {
            List<float> weights = new List<float>();
            foreach (var enemyExp in EnemyExperience) {
                float weight = 1f / enemyExp; // inverse weighing
                weights.Add(weight);
            }
            
            float totalExperience = 0f;
            for (int i = 0; i < maxEnemies; i++) {
                // weighted random selection
                int selectedEnemy = SelectEnemyBasedOnExperience(weights);
                float selectedEnemyExperience = EnemyExperience[selectedEnemy];
                if (totalExperience + selectedEnemyExperience > maxExperience)
                    break;
                waveEnemies.Add(selectedEnemy);
                totalExperience += selectedEnemyExperience;
            }
            
        }

        private int SelectEnemyBasedOnExperience(List<float> weights) {
            float totalWeight = 0f;
            foreach (var weight in weights) {
                totalWeight += weight;
            }
            float randomValue = Random.value * totalWeight;
            for (int i = 0; i < weights.Count; i++) {
                if (randomValue < weights[i])
                    return i;
                randomValue -= weights[i];
            }
            return Random.Range(0, weights.Count - 1);
        }

        private IEnumerator SpawnWave() {
            foreach (var enemy in waveEnemies) {
                SpawnEnemy(enemy);
                yield return new WaitForSeconds(Random.Range(3f, 10f));
            }
        }

        private void SpawnEnemy(int enemyIndex) {
            var enemyGameObject = Instantiate(EnemyPrefabs[enemyIndex], GetRandomSpawnPoint(), Quaternion.identity);
            var enemy = enemyGameObject.GetComponent<Enemy>();
            enemy.ExperienceValue = EnemyExperience[enemyIndex];
            enemy.SetTargets(enemyGameObject.GetType().Name == "Titan", player, cabin, target);
        }

        Vector3 GetRandomSpawnPoint()
        {
            float randomAngle = Random.Range(0f, Mathf.PI * 2);
            float randomDistance = Random.Range(MinSpawnDistance, MaxSpawnDistance);
            return new Vector3(Mathf.Cos(randomAngle) * randomDistance, 0f, Mathf.Sin(randomAngle) * randomDistance);
        }

        public void OnEnemyDestroyed() {
            enemyDeathCount++;
            if (enemyDeathCount >= waveEnemies.Count) {
                SiegeManager.Instance.Victory(); //TODO: next wave
            }
        }
        
        void OnValidate()
        {
            if (EnemyPrefabs.Length != EnemyExperience.Length)
                Array.Resize(ref EnemyExperience, EnemyPrefabs.Length);
        }
    }
}