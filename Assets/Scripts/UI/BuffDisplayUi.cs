using System.Text;
using TMPro;
using UnityEngine;

public class BuffDisplayUi : MonoBehaviour
{
    private TextMeshProUGUI buffText;
    private UpgradeBuffs buffs;

    void Awake()
    {
        // cache upgrade data
        buffText = GetComponent<TextMeshProUGUI>();
        buffs = FindFirstObjectByType<UpgradeBuffs>();
    }

    void Update()
    {
        if (buffText == null || buffs == null)
            return;

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Bonuses From Upgrades:");

        // note: formatting is just an example, you can tweak decimals or add labels as needed
        if (buffs.DrillHealthAddition != 0)
            stringBuilder.AppendLine($"Drill Health: +{buffs.DrillHealthAddition:F0}");
        if (buffs.DrillSpeedAddition != 0)
            stringBuilder.AppendLine($"Drill Speed: +{buffs.DrillSpeedAddition:F2}");
        if (buffs.DrillDamageMultiplier != 1)
            stringBuilder.AppendLine($"Drill Damage ×{buffs.DrillDamageMultiplier:F2}");
        if (buffs.WeaponDamageMultiplier != 1)
            stringBuilder.AppendLine($"Weapon Damage ×{buffs.WeaponDamageMultiplier:F2}");
        if (buffs.WeaponCooldownAddition != 0)
            stringBuilder.AppendLine($"Weapon Cooldown: {buffs.WeaponCooldownAddition:F2}s");
        if (buffs.WeaponRadiusAddition != 0)
            stringBuilder.AppendLine($"Weapon Radius: +{buffs.WeaponRadiusAddition:F2}");

        buffText.text = stringBuilder.ToString();
    }
}