using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [field: SerializeField] public double Money { get; private set; }
    private SerializedDictionary<string, int> MineralInventory { get; set; }

    //events
    public UnityEvent runStartEvent = new UnityEvent();
    public UnityEvent runStopEvent = new UnityEvent();

    private ShopUI Shop;

    //canvas groups
    private CanvasGroup preRunUI;
    private CanvasGroup midRunUI;
    public bool inRun = false;

    void Awake()
    {
        instance = this;

        preRunUI = GameObject.FindGameObjectWithTag("PreRunUI").GetComponent<CanvasGroup>();
        midRunUI = GameObject.FindGameObjectWithTag("MidRunUI").GetComponent<CanvasGroup>();
        Shop = FindFirstObjectByType<ShopUI>();

        ShowHomeUI();
        Money = 0;
        MineralInventory = new SerializedDictionary<string, int>();
    }

    //ai gen zone
    public void AddMoney(double amount)
    {
        Money += amount;
    }

    public bool RemoveMoney(double amount)
    {
        if (Money <= amount) return false;
        Money -= amount;
        return true;
    }

    public int GetMineralQuantity(string mineral)
    {
        if (MineralInventory.TryGetValue(mineral, out int qty))
            return qty;
        return 0;
    }

    public void AddMineral(string mineral, int amount)
    {
        if (MineralInventory.ContainsKey(mineral))
            MineralInventory[mineral] += amount;
        else
            MineralInventory[mineral] = amount;
    }

    public bool RemoveMineral(string mineral, int amount)
    {
        if (!MineralInventory.TryGetValue(mineral, out int current) || current < amount)
            return false;

        current -= amount;
        if (current > 0)
            MineralInventory[mineral] = current;
        else
            MineralInventory.Remove(mineral);

        return true;
    }


    private void ShowHomeUI()
    {

        //show home ui, hide mid-run ui
        preRunUI.alpha = 1;
        preRunUI.interactable = true;
        preRunUI.blocksRaycasts = true;

        midRunUI.alpha = 0;
        midRunUI.interactable = false;
        midRunUI.blocksRaycasts = false;

        //stock the shop
        Shop.SetShop();
    }

    private void ShowRunUI()
    {
        //show mid-run ui, hide home ui
        midRunUI.alpha = 1;
        midRunUI.interactable = true;
        midRunUI.blocksRaycasts = true;

        preRunUI.alpha = 0;
        preRunUI.interactable = false;
        preRunUI.blocksRaycasts = false;
    }

    public void StartRun()
    {
        ShowRunUI();
        inRun = true;
        MineralInventory = new SerializedDictionary<string, int>();
        runStartEvent.Invoke();
    }

    //we want to stop the game loop to show the game over screen and cash out
    public void StopRun()
    {
        inRun = false;
        runStopEvent.Invoke();
    }

    //and then through clicking cash out we end the run
    public void EndRun()
    {
        ShowHomeUI();
    }

}
