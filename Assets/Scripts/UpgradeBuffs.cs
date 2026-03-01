using System;
using UnityEngine;

public class UpgradeBuffs : MonoBehaviour
{
    public static UpgradeBuffs instance;

    public double DrillHealthAddition => PlayerDrill.instance.drillHealthLevel * 50;
    public double DrillSpeedAddition => PlayerDrill.instance.drillSpeedLevel * 0.5;
    public double DrillDamageMultiplier => Math.Pow(1.15, PlayerDrill.instance.drillDamageLevel);
    public double WeaponDamageMultiplier => Math.Pow(1.1, PlayerDrill.instance.weaponDamageLevel);
    public double WeaponCooldownAddition => PlayerDrill.instance.WeaponCooldownLevel * 0.1;
    public double WeaponRadiusAddition => PlayerDrill.instance.weaponRadiusLevel * 0.1;

    void Awake()
    {
        instance = this;
    }
}