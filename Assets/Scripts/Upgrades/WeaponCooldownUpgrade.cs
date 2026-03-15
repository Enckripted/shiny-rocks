using System;
using UnityEngine;

public class WeaponCooldownUpgrade : Upgrade
{
    [field: SerializeField] public double CooldownReductionPerLevel { get; private set; } = 0.1;

    public override PlayerStats ApplyEffect(PlayerStats curStats)
    {
        curStats.WeaponCooldown = Math.Max(0.01, curStats.WeaponCooldown - CooldownReductionPerLevel * Level);
        return curStats;
    }
}
