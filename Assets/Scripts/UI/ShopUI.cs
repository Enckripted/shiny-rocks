using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

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
        PanelLeft = transform.Find("OptionLeft").gameObject;
        PanelMiddle = transform.Find("OptionMiddle").gameObject;
        PanelRight = transform.Find("OptionRight").gameObject;

        player = FindFirstObjectByType<PlayerDrill>();

        // Initialize upgrade dictionary
        UpgradeList = new Dictionary<string, int>()
        {
            { "Drill Speed", 100 },
            { "Drill Damage", 100 },
            { "Weapon Damage", 100 }
        };
    }

    private Dictionary<string, int> SetItems()
    {
        CurrentShop.Clear();

        var keys = new List<string>(UpgradeList.Keys);
        string lastItemAdded = "";

        for (int i = 0; i < 3; i++)
        {
            string item;

            do
            {
                int randIndex = Random.Range(0, keys.Count);
                item = keys[randIndex];
            }
            while (item == lastItemAdded && keys.Count > 1);

            CurrentShop[item] = UpgradeList[item];
            lastItemAdded = item;
        }

        return CurrentShop;
    }

    public void SetShop()
    {
        var shopItems = SetItems();
        var keys = new List<string>(shopItems.Keys);

        PanelLeft.transform.Find("OptionText").GetComponent<TMP_Text>().text = keys[0];
        PanelMiddle.transform.Find("OptionText").GetComponent<TMP_Text>().text = keys[1];
        PanelRight.transform.Find("OptionText").GetComponent<TMP_Text>().text = keys[2];
    }

    public void ButtonPressed(string buttonName)
    {
        switch (buttonName)
        {
            case "OptionLeft":
                BuyUpgrade(
                    PanelLeft.transform.Find("OptionText").GetComponent<TMP_Text>().text,
                    int.Parse(PanelLeft.transform.Find("Cost").GetComponent<TMP_Text>().text[1..])
                );
                break;
            case "OptionMiddle":
                BuyUpgrade(
                    PanelMiddle.transform.Find("OptionText").GetComponent<TMP_Text>().text,
                    int.Parse(PanelMiddle.transform.Find("Cost").GetComponent<TMP_Text>().text[1..])
                );
                break;
            case "OptionRight":
                BuyUpgrade(
                    PanelRight.transform.Find("OptionText").GetComponent<TMP_Text>().text,
                    int.Parse(PanelRight.transform.Find("Cost").GetComponent<TMP_Text>().text[1..])
                );
                break;
        }
    }

    void BuyUpgrade(string upgradeName, int upgradeCost)
    {
        /*
        if(upgradeCost > GameManager.instance.Money)
        {
            Debug.Log("ur broke LOL... money: " + GameManager.instance.Money);
            return;
        }
        */

        Debug.Log("UpgradeName: " + upgradeName);

        switch (upgradeName)
        {
            //temp values
            case "Drill Speed":
                player.DrillSpeed += 1; 
                break;
            case "Drill Damage":
                player.DrillDamage += 1;
                break;
            case "Weapon Damage":
                FindFirstObjectByType<WeaponBase>().WeaponDamage += 1;
                break;
        }

    }

}