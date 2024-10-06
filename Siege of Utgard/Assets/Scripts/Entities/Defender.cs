using System;
using Entities.Enemies;
using UnityEngine;

namespace Game {
    public class Defender : Entity {
        public static Defender Instance { get; private set; }

        private float experience;
        private float currentExperience = 0f;
        
        [HideInInspector] public float Experience => experience;

        //buffs
        private float damageDone = 1f;
        private float experienceBuff = 0f;
        private float armorBuff = 1f;
        private float healthRegen = 1f;
        
        //debuffs
        private float damageTaken = 1f;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Persist across scenes if necessary
            }
            else {
                Destroy(gameObject);
            }
        }

        private void Update() {
            if (currentHealth < MaxHealth) {
                currentHealth += 0.1f * healthRegen;
            }
        }

        public override void TakeDamage(float amount) {
            amount = (amount * damageTaken) * (1f - armorBuff);
            base.TakeDamage(amount);
        }

        public void DealDamage(float amount, Enemy target) {
            var damage = amount * damageDone;
            target.TakeDamage(damage);
        }
        
        protected override void Die() {
            base.Die();
            //TODO: game over
        }
        
        #region Experience
        
        public bool SpendExperience(float amount) {
            if (currentExperience >= amount) {
                currentExperience -= amount;
                // Update UI
                return true;
            }
            return false;
        }
        
        public void GainExperience(float amount) {
            currentExperience += amount * experienceBuff;
            experience += amount * experienceBuff;
            // Update UI
        }

        #endregion

        #region Buffs

        public void IncreaseArmorBuff(float by) {
            armorBuff += by;
        }
        
        public void IncreaseHealthBuff(float by) {
            MaxHealth += by;
            currentHealth += by;
        }

        public void IncreaseDamageBuff(float by) {
            damageDone += by;
        }
        
        public void IncreaseExperienceBuff(float by) {
            experienceBuff += by;
        }
        
        public void IncreaseHealthRegen(float by) {
            healthRegen += by;
        }

        public void IncreaseDamageTaken(float by) {
            damageTaken += by;
        }

        #endregion
    }
}