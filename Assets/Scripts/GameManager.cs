using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public double Money { get; private set; }
    private SerializedDictionary<string, int> MineralInventory { get; set; }

    //canvas groups
    private CanvasGroup preRunUI;
    private CanvasGroup midRunUI;
    public bool inRun = false;

    void Awake()
    {
        instance = this;

        preRunUI = GameObject.FindGameObjectWithTag("PreRunUI").GetComponent<CanvasGroup>();
        midRunUI = GameObject.FindGameObjectWithTag("MidRunUI").GetComponent<CanvasGroup>();

        ShowHomeUI();
        Money = 0;
        MineralInventory = new SerializedDictionary<string, int>();

    }

    //ai gen zone
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
    }

    //we want to stop the game loop to show the game over screen and cash out
    public void StopRun()
    {
        inRun = false;
    }

    //and then through clicking cash out we end the run
    public void EndRun()
    {
        ShowHomeUI();
    }

}
