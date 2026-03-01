using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class BuffDisplayUi : MonoBehaviour
{
    private TextMeshProUGUI buffText;
    private UpgradeBuffs buffs;
    [SerializeField] private List<GameObject> panels;

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

        Debug.Log(panels[0].name);

        panels[0].GetComponent<TMP_Text>().text = "Drill Health: " + buffs.DrillHealthAddition;
        panels[1].GetComponent<TMP_Text>().text = "Drill Speed: " + buffs.DrillSpeedAddition;
        panels[2].GetComponent<TMP_Text>().text = "Drill Damage: " + buffs.DrillDamageMultiplier;
        panels[3].GetComponent<TMP_Text>().text = "Weapon Damage: " + buffs.WeaponDamageMultiplier;
        panels[4].GetComponent<TMP_Text>().text = "Weapon Cooldown: " + buffs.WeaponCooldownAddition;
        panels[5].GetComponent<TMP_Text>().text = "Weapon Radius: " + buffs.WeaponRadiusAddition;

    }
}