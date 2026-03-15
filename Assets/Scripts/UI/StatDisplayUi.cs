using TMPro;
using UnityEngine;

public class StatDisplayUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI depthText;
    [SerializeField] private TextMeshProUGUI moneyText;

    private PlayerDrill player;

    void Awake()
    {
        player = FindFirstObjectByType<PlayerDrill>();
    }

    void Update()
    {
        if (moneyText != null)
        {
            // whole numbers only
            double money = GameManager.instance != null ? GameManager.instance.Money : 0;
            moneyText.text = "$" + money.ToString("N0");
        }

        if (depthText != null && player != null)
        {
            depthText.text = player.DrillDepth.ToString("F1") + "m";
        }
    }
}