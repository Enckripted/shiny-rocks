using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class Damagable : MonoBehaviour
{
    [SerializeField] private HealthBar healthbar;
    [SerializeField] private GameObject damageNumberPrefab;
    [SerializeField] private bool showDamageNumbers;

    public float MaxHealth { get; protected set; }
    public float Health { get; protected set; }

    private Canvas uiCanvas;

    public UnityEvent OnDeathEvent;

    public void UpdateHealthbar()
    {
        healthbar.Health = Health;
        healthbar.MaxHealth = MaxHealth;
    }

    void HealHealth(float healAmount)
    {
        Health = Math.Min(Health + healAmount, MaxHealth);
        UpdateHealthbar();
    }

    public void DealDamage(float damage)
    {
        if (showDamageNumbers)
        {
            DamageNumber nDmgNumber = Instantiate(damageNumberPrefab, transform.position, Quaternion.identity).GetComponent<DamageNumber>();
            nDmgNumber.damageNum = Math.Floor(damage);
            //nDmgNumber.GetComponent<RectTransform>().position = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
        }

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

    void Start()
    {
        uiCanvas = FindAnyObjectByType<Canvas>();
    }
}
