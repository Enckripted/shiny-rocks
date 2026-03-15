using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class TooltipManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI bodyText;
    [SerializeField] private GraphicRaycaster graphicRaycaster;

    private CanvasGroup canvasGroup;

    private void ShowTooltip(Tooltip tooltip)
    {
        canvasGroup.alpha = 1;
        headerText.text = tooltip.Header;
        bodyText.text = tooltip.Body;
    }

    private void HideTooltip()
    {
        canvasGroup.alpha = 0;
    }

    private void SetPivot()
    {
        //TODO: set pivot to (1,0) if it would overflow as (0,1)
    }

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        HideTooltip();
    }

    void Update()
    {
        PointerEventData ped = new PointerEventData(null);
        ped.position = Mouse.current.position.value;

        transform.position = ped.position;

        List<RaycastResult> results = new();
        graphicRaycaster.Raycast(ped, results);

        foreach (RaycastResult res in results)
        {
            Tooltip tooltip = res.gameObject.GetComponent<Tooltip>();
            if (tooltip != null)
            {
                ShowTooltip(tooltip);
                SetPivot();
                return;
            }
        }
        HideTooltip();
    }
}