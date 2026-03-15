using System;
using UnityEngine;

public class WeaponDamageUpgrade : Upgrade
{
    [field: SerializeField] public double DamageMultiplierPerLevel { get; private set; } = 1.1;

    public override string EffectText => $"Currently: x{Math.Pow(DamageMultiplierPerLevel, Level):F2}";

    public override PlayerStats ApplyEffect(PlayerStats curStats)
    {
        curStats.WeaponDamage *= Math.Pow(DamageMultiplierPerLevel, Level);
        return curStats;
    }
}
