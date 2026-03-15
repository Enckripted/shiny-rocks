using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[RequireComponent(typeof(Tooltip))]
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(LayoutElement))]
public class UpgradeBuffItem : MonoBehaviour
{
    public Upgrade SelectedUpgrade { get; set; }

    [SerializeField] private TextMeshProUGUI symbolDisplay;

    private Tooltip tooltip;
    private CanvasGroup canvasGroup;
    private LayoutElement layoutElement;

    private bool shouldShow => SelectedUpgrade.Level > 0;

    private void UpdateTooltip()
    {
        tooltip.Header = SelectedUpgrade.Name;

        StringBuilder body = new StringBuilder();
        body.AppendLine(SelectedUpgrade.Description);
        body.AppendLine($"Level: {SelectedUpgrade.Level}");
        if (SelectedUpgrade.EffectText != "")
            body.AppendLine(SelectedUpgrade.EffectText);
        tooltip.Body = body.ToString();
    }

    void Awake()
    {
        tooltip = GetComponent<Tooltip>();
        canvasGroup = GetComponent<CanvasGroup>();
        layoutElement = GetComponent<LayoutElement>();
    }

    void Update()
    {
        canvasGroup.alpha = shouldShow ? 1 : 0;
        canvasGroup.blocksRaycasts = shouldShow;
        layoutElement.ignoreLayout = !shouldShow;

        UpdateTooltip();
        symbolDisplay.text = SelectedUpgrade.Symbol;
    }
}