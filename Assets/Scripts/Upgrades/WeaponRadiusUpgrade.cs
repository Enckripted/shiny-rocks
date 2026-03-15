using System;
using UnityEngine;

public class WeaponRadiusUpgrade : Upgrade
{
    [field: SerializeField] public double RadiusPerLevel { get; private set; } = 0.1;

    public override PlayerStats ApplyEffect(PlayerStats curStats)
    {
        curStats.BaseWeaponRadius += RadiusPerLevel * Level;
        return curStats;
    }
}
