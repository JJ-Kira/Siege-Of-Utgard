using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public float MaxHealth = 100;
    private float currentHealth;
    
    protected virtual void Start() {
        currentHealth = MaxHealth;
    }
    
    public void TakeDamage(float amount) {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
        if (currentHealth <= 0) {
            Die();
        }
    }

    public void Heal(float amount) {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
    }

    protected virtual void Die() {
        Debug.Log($"{gameObject.name} has died!");
    }
}
