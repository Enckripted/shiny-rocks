using System;
using UnityEngine;

public class WeaponDamageUpgrade : Upgrade
{
    [field: SerializeField] public double DamageMultiplierPerLevel { get; private set; } = 1.1;

    public override PlayerStats ApplyEffect(PlayerStats curStats)
    {
        curStats.BaseWeaponDamage *= Math.Pow(DamageMultiplierPerLevel, Level);
        return curStats;
    }
}
