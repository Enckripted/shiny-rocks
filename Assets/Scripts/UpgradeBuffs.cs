using System;
using UnityEngine;

public class UpgradeBuffs : MonoBehaviour
{
    public static UpgradeBuffs instance;

    private PlayerDrill playerDrill;

    public double DrillHealthAddition => playerDrill.drillHealthLevel * 50;
    public double DrillSpeedAddition => playerDrill.drillSpeedLevel * 0.5;
    public double DrillDamageMultiplier => Math.Pow(1.15, playerDrill.drillDamageLevel);
    public double WeaponDamageMultiplier => Math.Pow(1.1, playerDrill.weaponDamageLevel);
    public double WeaponCooldownAddition => playerDrill.WeaponCooldownLevel * 0.1;
    public double WeaponRadiusAddition => playerDrill.weaponRadiusLevel * 0.1;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        playerDrill = FindFirstObjectByType<PlayerDrill>();
    }
}