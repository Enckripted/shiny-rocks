using System.Collections.Generic;
using UnityEngine;

public class PlayerDrill : Damagable
{
    public static PlayerDrill instance;

    public double InitialHealth => BaseHealth + UpgradeBuffs.instance.DrillHealthAddition;
    public double DrillSpeed => BaseDrillSpeed + UpgradeBuffs.instance.DrillSpeedAddition;
    public double DrillDamage => BaseDrillDamage * UpgradeBuffs.instance.DrillDamageMultiplier;
    public double WeaponDamage => BaseWeaponDamage * UpgradeBuffs.instance.WeaponDamageMultiplier;
    public double WeaponRadius => BaseWeaponRadius + UpgradeBuffs.instance.WeaponRadiusAddition;
    public double WeaponCooldown => BaseWeaponCooldown + UpgradeBuffs.instance.WeaponCooldownAddition;

    public float DrillDepth;

    [Header("Base Stats")]
    public double BaseHealth;
    public double BaseDrillSpeed;
    public double BaseDrillDamage;
    public double BaseWeaponDamage;
    public double BaseWeaponRadius;
    public double BaseWeaponCooldown;

    [Header("Levels")]
    public int drillHealthLevel = 0;
    public int drillSpeedLevel = 0;
    public int drillDamageLevel = 0;
    public int weaponDamageLevel = 0;
    public int WeaponCooldownLevel = 0;
    public int weaponRadiusLevel = 0;

    public bool IsMoving;

    private Collider2D drillCollider;
    private List<Mineral> collidingMinerals = new List<Mineral>();

    private void StopRun()
    {
        GameManager.instance.StopRun();
    }

    private void OnRunBegin()
    {
        Health = (float)InitialHealth;
        MaxHealth = (float)InitialHealth;
        DrillDepth = 0;
        UpdateHealthbar();
    }

    void Start()
    {
        OnDeathEvent.AddListener(StopRun);
        GameManager.instance.runStartEvent.AddListener(OnRunBegin);
        Health = 1;
        MaxHealth = 1;
        UpdateHealthbar();
        //OnRunBegin();
        instance = this;
    }

    void Update()
    {
        List<Mineral> remainingMinerals = new List<Mineral>();
        foreach (Mineral mineral in collidingMinerals)
        {
            mineral.DealDamage((float)DrillDamage * Time.deltaTime);
            if (mineral.Health > 0)
                remainingMinerals.Add(mineral);
        }
        collidingMinerals = remainingMinerals;
        IsMoving = collidingMinerals.Count == 0;

        if (IsMoving)
        {
            DrillDepth += (float)DrillSpeed * Time.deltaTime / 10;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Mineral mineral = collision.collider.gameObject.GetComponent<Mineral>();
        if (mineral == null)
            return;

        if (DrillDamage / 10 >= mineral.Health)
            mineral.DealDamage((float)DrillDamage);
        if (mineral.Health > 0)
            collidingMinerals.Add(mineral);
    }
}
