using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public struct Cost
{
    public double BaseCost;
    public double CostScale;
}

public class ShopUI : MonoBehaviour
{
    private PlayerDrill player;

    [SerializeField] GameObject PanelLeft;
    [SerializeField] GameObject PanelMiddle;
    [SerializeField] GameObject PanelRight;

    // Master upgrade list (name → value)
    private Dictionary<string, Cost> UpgradeList = new();

    // Current shop selection (name → value)
    private Dictionary<string, Cost> CurrentShop = new();

    void Awake()
    {
        PanelLeft = transform.Find("OptionLeft")?.gameObject;
        PanelMiddle = transform.Find("OptionMiddle")?.gameObject;
        PanelRight = transform.Find("OptionRight")?.gameObject;

        player = FindFirstObjectByType<PlayerDrill>();
    }

    void SetUpgradeList()
    {
        UpgradeList = new Dictionary<string, Cost>()
        {
            { "Drill Speed", new Cost{ BaseCost = 15, CostScale = 1.15} },
            { "Drill Damage", new Cost{ BaseCost = 10, CostScale = 1.15} },
            { "Drill Health", new Cost{ BaseCost = 10, CostScale = 1.15} },
            { "Weapon Damage", new Cost{ BaseCost = 10, CostScale = 1.15} },
            { "Weapon Cooldown", new Cost{ BaseCost = 25, CostScale = 1.15}},
            { "Weapon Radius", new Cost{ BaseCost = 50, CostScale = 1.15}}
        };
    }

    private double getLevelsFromUpgradeName(string upgradeName)
    {
        switch (upgradeName)
        {
            case "Drill Speed":
                return player.drillSpeedLevel;
            case "Drill Damage":
                return player.drillDamageLevel;
            case "Drill Health":
                return player.drillHealthLevel;
            case "Weapon Damage":
                return player.weaponDamageLevel;
            case "Drill Cooldown":
                return player.WeaponCooldownLevel;
            case "Drill Radius":
                return player.weaponRadiusLevel;
        }
        return 0;
    }

    private Dictionary<string, Cost> SetItems()
    {
        if (UpgradeList == null || UpgradeList.Count == 0)
        {
            SetUpgradeList();
        }

        CurrentShop.Clear();

        // Create a list of keys and shuffle it
        var keys = new List<string>(UpgradeList.Keys);
        Shuffle(keys);

        // Pick up to 4 unique upgrades
        int itemsToAdd = Mathf.Min(6, keys.Count);
        for (int i = 0; i < itemsToAdd; i++)
        {
            CurrentShop[keys[i]] = UpgradeList[keys[i]];
        }

        return CurrentShop;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randIndex = UnityEngine.Random.Range(i, list.Count);
            (list[randIndex], list[i]) = (list[i], list[randIndex]);
        }
    }

    public void SetShop()
    {
        var shopItems = SetItems();
        var keys = new List<string>(shopItems.Keys);

        // Safe filling of panels
        SetPanel(PanelLeft, keys, 0);
        SetPanel(PanelMiddle, keys, 1);
        SetPanel(PanelRight, keys, 2);
    }

    private void SetPanel(GameObject panel, List<string> keys, int index)
    {
        if (panel == null) return;

        var nameText = panel.transform.Find("OptionText").GetComponent<TMP_Text>();
        var costText = panel.transform.Find("Cost").GetComponent<TMP_Text>();

        if (index >= keys.Count)
        {
            if (nameText != null) nameText.text = "N/A";
            if (costText != null) costText.text = "$0";
            return;
        }

        string upgradeName = keys[index];
        int price = (int)Math.Floor(UpgradeList[upgradeName].BaseCost * Math.Pow(UpgradeList[upgradeName].CostScale, getLevelsFromUpgradeName(upgradeName)));

        if (nameText != null)
            nameText.text = upgradeName;

        if (costText != null)
            costText.text = "$" + price;
    }

    public void ButtonPressed(string buttonName)
    {
        GameObject panel = buttonName switch
        {
            "OptionLeft" => PanelLeft,
            "OptionMiddle" => PanelMiddle,
            "OptionRight" => PanelRight,
            _ => null
        };

        if (panel == null) return;

        var textComp = panel.transform.Find("OptionText").GetComponent<TMP_Text>();
        var costComp = panel.transform.Find("Cost").GetComponent<TMP_Text>();

        if (!int.TryParse(costComp.text[1..], out int cost))
        {
            Debug.LogWarning($"Invalid cost format in {panel.name}");
            return;
        }

        if (GameManager.instance.Money < cost)
            return;

        BuyUpgrade(textComp.text, cost);
        SetShop();
    }

    void BuyUpgrade(string upgradeName, int upgradeCost)
    {

        GameManager.instance.RemoveMoney(upgradeCost);

        switch (upgradeName)
        {
            case "Drill Speed":
                player.drillSpeedLevel++;
                break;
            case "Drill Damage":
                player.drillDamageLevel++;
                break;
            case "Drill Health":
                player.drillHealthLevel++;
                break;
            case "Weapon Damage":
                player.weaponDamageLevel++;
                break;
            case "Drill Cooldown":
                player.WeaponCooldownLevel++;
                break;
            case "Drill Radius":
                player.weaponRadiusLevel++;
                break;
        }
    }
}