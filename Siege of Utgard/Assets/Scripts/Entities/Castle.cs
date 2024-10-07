using Game;

namespace Entities {
    public class Castle : Entity {
        public static Castle Instance { get; private set; }
        
        private float armorBuff = 0f;
        private void Awake() {
            if (Instance == null) {
                Instance = this;
                //DontDestroyOnLoad(gameObject); // Persist across scenes if necessary
            }
            else {
                Destroy(gameObject);
            }
        }

        public override void TakeDamage(float amount) {
            amount = amount * (1f - armorBuff);
            base.TakeDamage(amount);
        }
        
        protected override void Die()
        {
            base.Die();
            Defender.Instance.CastleFell();
        }

        #region Buffs
        
        public void IncreaseArmorBuff(float by)
        {
            armorBuff += by;
        }
        
        public void IncreaseHealthBuff(float by)
        {
            MaxHealth += by;
            currentHealth += by;
        }
        
        #endregion
    }
}
