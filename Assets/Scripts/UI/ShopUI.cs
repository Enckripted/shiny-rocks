using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopUI : MonoBehaviour
{
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

        PanelLeft.transform.Find("OptionText")
            .GetComponent<TMP_Text>().text = keys[0];

        PanelMiddle.transform.Find("OptionText")
            .GetComponent<TMP_Text>().text = keys[1];

        PanelRight.transform.Find("OptionText")
            .GetComponent<TMP_Text>().text = keys[2];
    }
}