using System;
using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{
    public int Level { get; private set; }
    public float Cost { get => GetCost(); }

    [field: SerializeField] public string Symbol { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }

    [field: SerializeField] public float InitialCost { get; private set; }
    [field: SerializeField] public float CostScale { get; private set; }

    public abstract PlayerStats ApplyEffect(PlayerStats curStats);

    private float GetCost()
    {
        return InitialCost * (float)Math.Pow(CostScale, Level);
    }

    public void LevelUp()
    {
        Level++;
    }

    public void LevelDown()
    {
        Level--;
    }
}