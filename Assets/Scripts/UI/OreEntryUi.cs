using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(LayoutElement))]
public class OreEntryUi : MonoBehaviour
{
    public MineralData MineralData { get; set; }

    [SerializeField] private Image oreImage;
    [SerializeField] private TextMeshProUGUI oreNameText;
    [SerializeField] private TextMeshProUGUI oreAmount;

    private CanvasGroup canvasGroup;
    private LayoutElement layoutElement;
    private GameManager gameManager;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        layoutElement = GetComponent<LayoutElement>();
    }

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        oreImage.color = MineralData.Color;
        oreNameText.text = MineralData.Name;
    }

    void Update()
    {
        canvasGroup.alpha = gameManager.GetMineralQuantity(MineralData.Name) > 0 ? 1 : 0;
        layoutElement.ignoreLayout = gameManager.GetMineralQuantity(MineralData.Name) <= 0;
        oreAmount.text = gameManager.GetMineralQuantity(MineralData.Name).ToString();
    }
}