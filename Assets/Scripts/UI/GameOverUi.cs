using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class GameOverUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI revenueBreakdownText;
    [SerializeField] private TextMeshProUGUI revenueText;
    [SerializeField] private Button closeButton;

    private CanvasGroup canvasGroup;

    private void RefreshRevenue()
    {
        if (revenueBreakdownText == null || revenueText == null)
            return;

        double total = 0;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        // iterate through all known minerals
        foreach (MineralData md in MineralDataStore.instance.Minerals)
        {
            int qty = GameManager.instance.GetMineralQuantity(md.Name);
            if (qty > 0)
            {
                double value = qty * md.SellValue;
                total += value;
                sb.AppendLine($"{md.Name}: {qty} × {md.SellValue} = ${value}");
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

    private void Close()
    {
        // fade out using the CanvasGroup component so the object remains active
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(Close);

        RefreshRevenue();
    }
}