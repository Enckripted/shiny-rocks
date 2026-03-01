using System.Collections.Generic;
using UnityEngine;

public class PlayerDrill : Damagable
{
    public int InitialHealth;
    public float DrillSpeed;
    public float DrillDamage;
    public float WeaponDamage;
    public float DrillDepth;

    public bool IsMoving;

    private Collider2D drillCollider;
    private List<Mineral> collidingMinerals = new List<Mineral>();

    private void StopRun()
    {
        GameManager.instance.StopRun();
    }

    private void OnRunBegin()
    {
        Health = InitialHealth;
        MaxHealth = InitialHealth;
        DrillDepth = 0;
        UpdateHealthbar();
    }

    void Start()
    {
        OnDeathEvent.AddListener(StopRun);
        GameManager.instance.runStartEvent.AddListener(OnRunBegin);
        OnRunBegin();
    }

    void Update()
    {
        List<Mineral> remainingMinerals = new List<Mineral>();
        foreach (Mineral mineral in collidingMinerals)
        {
            mineral.DealDamage(DrillDamage * Time.deltaTime);
            if (mineral.Health > 0)
                remainingMinerals.Add(mineral);
        }
        collidingMinerals = remainingMinerals;
        IsMoving = collidingMinerals.Count == 0;

        if (IsMoving)
        {
            DrillDepth += DrillSpeed * Time.deltaTime / 10;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Mineral mineral = collision.collider.gameObject.GetComponent<Mineral>();
        if (mineral == null)
            return;

        mineral.DealDamage(DrillDamage);
        if (mineral.Health > 0)
            collidingMinerals.Add(mineral);
    }
}
