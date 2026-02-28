using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class Damagable : MonoBehaviour
{
    [SerializeField] private HealthBar healthbar;

    public abstract float MaxHealth { get; set; }
    public float Health { get; set; }

    public UnityEvent OnDeathEvent;

    void UpdateHealthbar()
    {
        healthbar.Health = Health;
        healthbar.MaxHealth = MaxHealth;
    }

    void HealHealth(float healAmount)
    {
        Health = Math.Min(Health + healAmount, MaxHealth);
        UpdateHealthbar();
    }

    void DealDamage(float damage)
    {
        Health = Math.Max(Health - damage, 0);
        UpdateHealthbar();

        if (Health == 0)
        {
            OnDeathEvent.Invoke();
        }
    }

    void Awake()
    {
        Health = MaxHealth;
        UpdateHealthbar();

        OnDeathEvent = new UnityEvent();
    }
}
