using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float currentHealth;
    public float CurrentHealth => currentHealth;
    [SerializeField]
    private float maxHealth;
    public float MaxHealth => maxHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float value)
    {
        currentHealth -= value;
        if (currentHealth <= 0)
        {
            Destroy();
        }
    }

    public void RestoreHealth(float value)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
        }
    }

    public virtual void Destroy()
    {
        currentHealth = 0;
        Destroy(gameObject);
    }
}
