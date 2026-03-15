using System;
using UnityEngine;

public class DrillDamageUpgrade : Upgrade
{
    [field: SerializeField] public double DamageMultiplierPerLevel { get; private set; } = 1.15;

    public override PlayerStats ApplyEffect(PlayerStats curStats)
    {
        curStats.BaseDrillDamage *= Math.Pow(DamageMultiplierPerLevel, Level);
        return curStats;
    }
}
