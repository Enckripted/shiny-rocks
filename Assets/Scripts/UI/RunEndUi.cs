using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class RunEndUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI revenueBreakdownText;
    [SerializeField] private TextMeshProUGUI revenueText;
    [SerializeField] private Button cashOutButton;

    private CanvasGroup canvasGroup;

    private double CalculateRevenue()
    {
        double total = 0;
        foreach (MineralData md in MineralDataStore.instance.Minerals)
        {
            double value = GameManager.instance.GetMineralQuantity(md.Name) * md.SellValue;
            total += value;
        }
        return total;
    }

    //ai gen
    private void RefreshRevenue()
    {
        if (revenueBreakdownText == null || revenueText == null)
            return;

        double total = 0;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("You mined out:");

        // iterate through all known minerals
        foreach (MineralData md in MineralDataStore.instance.Minerals)
        {
            int qty = GameManager.instance.GetMineralQuantity(md.Name);
            if (qty > 0)
            {
                double value = qty * md.SellValue;
                total += value;
                sb.AppendLine($"{md.Name}: {qty} × ${md.SellValue} = ${value}");
            }
        }

        revenueBreakdownText.text = sb.ToString();
        revenueText.text = "$" + total.ToString("N0");
    }

    private void Open()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        RefreshRevenue();
    }

    private void CashOut()
    {
        GameManager.instance.AddMoney(CalculateRevenue());

        // fade out using the CanvasGroup component so the object remains active
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        GameManager.instance.EndRun();
    }

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        GameManager.instance.runStopEvent.AddListener(Open);
        cashOutButton.onClick.AddListener(CashOut);
        RefreshRevenue();
    }
}