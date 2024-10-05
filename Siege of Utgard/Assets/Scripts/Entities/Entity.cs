using Unity.VisualScripting;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public float MaxHealth = 100;
    [Serialize] protected float currentHealth;
    
    protected virtual void Start() {
        currentHealth = MaxHealth;
    }
    
    public virtual void TakeDamage(float amount) {
        currentHealth -= amount;
        if (currentHealth <= 0) {
            Die();
        }
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
    }

    public void Heal(float amount) {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
    }

    protected virtual void Die() {
        Debug.Log($"{gameObject.name} has died!");
    }
}
