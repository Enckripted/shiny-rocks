using System;
using UnityEngine;

public class WeaponCooldownUpgrade : Upgrade
{
    [field: SerializeField] public double CooldownReductionPerLevel { get; private set; } = 0.1;

    public override PlayerStats ApplyEffect(PlayerStats curStats)
    {
        curStats.BaseWeaponCooldown = Math.Max(0.01, curStats.BaseWeaponCooldown - CooldownReductionPerLevel * Level);
        return curStats;
    }
}
