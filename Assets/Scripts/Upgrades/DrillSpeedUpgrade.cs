using System;
using UnityEngine;

public class DrillSpeedUpgrade : Upgrade
{
    [field: SerializeField] public double SpeedPerLevel { get; private set; } = 0.5;

    public override PlayerStats ApplyEffect(PlayerStats curStats)
    {
        curStats.DrillSpeed += SpeedPerLevel * Level;
        return curStats;
    }
}
