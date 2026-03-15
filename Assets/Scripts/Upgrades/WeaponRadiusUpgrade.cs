using System;
using UnityEngine;

public class WeaponRadiusUpgrade : Upgrade
{
    [field: SerializeField] public double RadiusPerLevel { get; private set; } = 0.1;

    public override string EffectText => $"Currently: +{RadiusPerLevel * Level:F2}";

    public override PlayerStats ApplyEffect(PlayerStats curStats)
    {
        curStats.WeaponRadius += RadiusPerLevel * Level;
        return curStats;
    }
}
