using UnityEngine;

public class OreEntryListUi : MonoBehaviour
{
    [SerializeField] private GameObject oreEntryUiPrefab;

    //ai generated

    void Start()
    {
        // Load all MineralData assets located in Resources/Minerals
        MineralData[] minerals = Resources.LoadAll<MineralData>("Minerals");
        if (minerals == null || minerals.Length == 0)
        {
            Debug.LogWarning("No MineralData found in Resources/Minerals");
            return;
        }

        foreach (MineralData data in minerals)
        {
            GameObject entryObj = Instantiate(oreEntryUiPrefab, transform);
            OreEntryUi entryUi = entryObj.GetComponent<OreEntryUi>();
            if (entryUi == null)
            {
                Debug.LogError("OreEntryUi component missing on prefab");
                continue;
            }
            entryUi.MineralData = data;
        }
    }
}