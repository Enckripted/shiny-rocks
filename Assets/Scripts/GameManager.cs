using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public double Money { get; private set; }
    public SerializedDictionary<string, int> MineralInventory { get; private set; }

    void Awake()
    {
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
}