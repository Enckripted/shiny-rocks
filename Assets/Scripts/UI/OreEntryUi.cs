using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class OreEntryUi : MonoBehaviour
{
    public MineralData MineralData { get; set; }

    [SerializeField] private Image oreImage;
    [SerializeField] private TextMeshProUGUI oreNameText;
    [SerializeField] private TextMeshProUGUI oreAmount;

    private CanvasGroup canvasGroup;
    private GameManager gameManager;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
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
        oreAmount.text = gameManager.GetMineralQuantity(MineralData.Name).ToString();
    }
}