using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

//ai gen with most null checks removed (we don't want to silently fail)
public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject PanelLeft;
    [SerializeField] private GameObject PanelMiddle;
    [SerializeField] private GameObject PanelRight;

    private readonly List<Upgrade> currentShop = new List<Upgrade>();

    public void SetShop()
    {
        currentShop.Clear();

        currentShop.AddRange(UpgradeManager.instance.Upgrades);
        Shuffle(currentShop);

        // keep exactly three panel options
        while (currentShop.Count > 3)
            currentShop.RemoveAt(currentShop.Count - 1);

        SetPanel(PanelLeft, currentShop[0]);
        SetPanel(PanelMiddle, currentShop[1]);
        SetPanel(PanelRight, currentShop[2]);
    }

    private void SetPanel(GameObject panel, Upgrade upgrade)
    {
        var nameText = panel.transform.Find("OptionText")?.GetComponent<TMP_Text>();
        var costText = panel.transform.Find("Cost")?.GetComponent<TMP_Text>();

        nameText.text = upgrade.Name;
        costText.text = "$" + Math.Ceiling(upgrade.Cost).ToString("F0");
    }

    public void ButtonPressed(string buttonName)
    {
        int index = buttonName switch
        {
            "OptionLeft" => 0,
            "OptionMiddle" => 1,
            "OptionRight" => 2,
            _ => -1
        };

        var selectedUpgrade = currentShop[index];
        int price = (int)Math.Ceiling(selectedUpgrade.Cost);

        if (GameManager.instance.Money < price)
            return;

        GameManager.instance.RemoveMoney(price);
        selectedUpgrade.LevelUp();

        SetShop();
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    void Awake()
    {
        PanelLeft = transform.Find("OptionLeft")?.gameObject;
        PanelMiddle = transform.Find("OptionMiddle")?.gameObject;
        PanelRight = transform.Find("OptionRight")?.gameObject;
    }
}
