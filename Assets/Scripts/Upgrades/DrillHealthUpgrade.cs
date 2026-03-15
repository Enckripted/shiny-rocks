using System;
using UnityEngine;

public class DrillHealthUpgrade : Upgrade
{
    [field: SerializeField] public double HealthPerLevel { get; private set; } = 50.0;

    public override string EffectText => $"Currently: +{HealthPerLevel * Level:F0}";

    public override PlayerStats ApplyEffect(PlayerStats curStats)
    {
        curStats.InitialHealth += HealthPerLevel * Level;
        return curStats;
    }
}
