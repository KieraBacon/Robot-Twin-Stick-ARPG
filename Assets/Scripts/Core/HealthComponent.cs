using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
    public delegate void DamageTakenEventHandler(GameObject caller, float damage, float currentHealth, float maxHealth);
    public delegate void DeathEventHandler(GameObject caller);
    public event DamageTakenEventHandler onDamageTaken;
    public event DeathEventHandler onDeath;

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

    public virtual void TakeDamage(GameObject damageDealer, float value)
    {
        currentHealth -= value;
        onDamageTaken?.Invoke(this.gameObject, value, currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Destroy();
        }
    }

    public virtual void RestoreHealth(float value)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
        }
    }

    public void RestoreHealth()
    {
        RestoreHealth(maxHealth);
    }

    public virtual void Destroy()
    {
        onDeath?.Invoke(this.gameObject);
        currentHealth = 0;
        //Destroy(gameObject);
    }
}
