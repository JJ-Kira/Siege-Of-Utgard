using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int MaxHealth = 50;
    protected int currentHealth;

    public int AttackPower = 10;
    public float speed = 3.5f;

    [Header("Pathfinding")]
    protected NavMeshAgent agent;
    public Transform Target; // Typically, the player's base or a specific point

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        currentHealth = MaxHealth;
        if (Target == null)
        {
            Target = GameObject.FindWithTag("EnemyTarget").transform;
        }
        SetDestination();
        SetDestination();
    }

    protected virtual void Update()
    {
        // You can add common behaviors here
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
        // Add UI update or death logic here
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        WaveManager waveManager = FindObjectOfType<WaveManager>();
        if (waveManager != null)
        {
            waveManager.OnEnemyDestroyed();
        }
        Debug.Log($"{gameObject.name} has died!");
        Destroy(gameObject);
    }

    protected void SetDestination()
    {
        if (Target != null)
        {
            agent.SetDestination(Target.position);
        }
    }

    // Abstract method for enemy-specific behavior
    public abstract void PerformAttack();
}