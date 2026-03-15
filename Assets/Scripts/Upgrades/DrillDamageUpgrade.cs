using System;
using UnityEngine;

public class DrillDamageUpgrade : Upgrade
{
    [field: SerializeField] public double DamageMultiplierPerLevel { get; private set; } = 1.15;

    public override string EffectText => $"Currently: x{Math.Pow(DamageMultiplierPerLevel, Level):F2}";

    public override PlayerStats ApplyEffect(PlayerStats curStats)
    {
        curStats.DrillDamage *= Math.Pow(DamageMultiplierPerLevel, Level);
        return curStats;
    }
}
