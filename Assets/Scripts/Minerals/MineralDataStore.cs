using System.Collections.Generic;
using UnityEngine;

public class MineralDataStore : MonoBehaviour
{
    private static MineralDataStore _instance;
    public static MineralDataStore instance
    {
        get
        {
            if (_instance == null)
            {
                // try to find an existing instance in the scene
                _instance = FindFirstObjectByType<MineralDataStore>();
                if (_instance == null)
                {
                    // create a new GameObject if none exist
                    GameObject go = new GameObject("MineralDataStore");
                    _instance = go.AddComponent<MineralDataStore>();
                }
            }
            return _instance;
        }
    }

    // publicly accessible compiled data
    public List<MineralData> Minerals { get; private set; } = new List<MineralData>();

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        LoadMinerals();
    }

    private void LoadMinerals()
    {
        Minerals.Clear();
        MineralData[] loaded = Resources.LoadAll<MineralData>("Minerals");
        if (loaded != null && loaded.Length > 0)
        {
            Minerals.AddRange(loaded);
        }
        else
        {
            Debug.LogWarning("MineralDataStore: no mineral assets found in Resources/Minerals");
        }
    }
}