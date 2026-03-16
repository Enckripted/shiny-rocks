using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    [SerializeField] private GameObject upgradesObject;

    public List<Upgrade> Upgrades { get; private set; }

    public PlayerStats CalculateEffects(PlayerStats curStats)
    {
        foreach (Upgrade upgrade in Upgrades)
        {
            curStats = upgrade.ApplyEffect(curStats);
        }
        return curStats;
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Upgrades = new List<Upgrade>(upgradesObject.GetComponentsInChildren<Upgrade>());
    }
}