using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopUI : MonoBehaviour
{
    private PlayerDrill player;

    [SerializeField] GameObject PanelLeft;
    [SerializeField] GameObject PanelMiddle;
    [SerializeField] GameObject PanelRight;

    // Master upgrade list (name → value)
    private Dictionary<string, int> UpgradeList;

    // Current shop selection (name → value)
    private Dictionary<string, int> CurrentShop = new();

    void Awake()
    {
        PanelLeft = transform.Find("OptionLeft")?.gameObject;
        PanelMiddle = transform.Find("OptionMiddle")?.gameObject;
        PanelRight = transform.Find("OptionRight")?.gameObject;

        player = FindFirstObjectByType<PlayerDrill>();

        // Initialize upgrade dictionary
        UpgradeList = new Dictionary<string, int>();
    }

    void SetUpgradeList()
    {
        UpgradeList = new Dictionary<string, int>()
        {
            { "Drill Speed", 100 },
            { "Drill Damage", 100 },
            { "Weapon Damage", 100 }
        };
    }

    private Dictionary<string, int> SetItems()
    {
        if (UpgradeList == null || UpgradeList.Count == 0)
        {
            SetUpgradeList();
        }

        CurrentShop.Clear();

        // Create a list of keys and shuffle it
        var keys = new List<string>(UpgradeList.Keys);
        Shuffle(keys);

        // Pick up to 3 unique upgrades
        int itemsToAdd = Mathf.Min(3, keys.Count);
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
            int randIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
    }

    public void SetShop()
    {
        var shopItems = SetItems();
        var keys = new List<string>(shopItems.Keys);

        // Safe filling of panels
        SetPanelText(PanelLeft, keys, 0);
        SetPanelText(PanelMiddle, keys, 1);
        SetPanelText(PanelRight, keys, 2);
    }

    private void SetPanelText(GameObject panel, List<string> keys, int index)
    {
        if (panel == null) return;  // panel missing

        var textComp = panel.GetComponentInChildren<TMP_Text>();
        if (textComp == null)
        {
            Debug.LogWarning($"TMP_Text not found in panel {panel.name}");
            return;
        }

        textComp.text = (index < keys.Count) ? keys[index] : "N/A";
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

        var textComp = panel.transform.Find("OptionText")?.GetComponent<TMP_Text>();
        var costComp = panel.transform.Find("Cost")?.GetComponent<TMP_Text>();

        if (textComp == null || costComp == null)
        {
            Debug.LogWarning($"Missing OptionText or Cost in {panel.name}");
            return;
        }

        if (!int.TryParse(costComp.text[1..], out int cost))
        {
            Debug.LogWarning($"Invalid cost format in {panel.name}");
            return;
        }

        BuyUpgrade(textComp.text, cost);
    }

    void BuyUpgrade(string upgradeName, int upgradeCost)
    {
        Debug.Log($"Buying upgrade: {upgradeName} for {upgradeCost}");

        switch (upgradeName)
        {
            case "Drill Speed":
                player.DrillSpeed += 1; 
                break;
            case "Drill Damage":
                player.DrillDamage += 1;
                break;
            case "Weapon Damage":
                FindFirstObjectByType<WeaponBase>().WeaponDamage++;
                break;
            default:
                Debug.LogWarning($"Unknown upgrade: {upgradeName}");
                break;
        }
    }
}