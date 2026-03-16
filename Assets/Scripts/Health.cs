using System;
using UnityEngine;

public class Health : MonoBehaviour
{
	public float MaxHealth;
	[field: SerializeField] public float CurrentHealth { get; private set; } //serialized just to see in the inspector

	public event Action<float> OnDamage;
	public event Action<float> OnHeal;
	public event Action OnDeath;

	public void TakeDamage(float amount)
	{
		CurrentHealth -= amount;
		CurrentHealth = Mathf.Max(CurrentHealth, 0);
		OnDamage?.Invoke(amount);
		if (CurrentHealth == 0)
		{
			//if an event isnt subscribed to in c#, then it is null, this will
			//not invoke if it is null
			OnDeath?.Invoke();
		}
	}

	public void Heal(float amount)
	{
		CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
		OnHeal?.Invoke(amount);
	}

	public void SetMaxHealth(float amount)
	{
		MaxHealth = amount;
		CurrentHealth = amount;
	}
}