using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralSpawnManager : MonoBehaviour
{
    [SerializeField] private MineralSpawner spawner;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float spawnDistance = 0.5f;
    private Vector2 ySpawnRange = new(-2, 4);

    private MineralData[] allMineralData;
    private Camera mainCam;
    private PlayerDrill playerDrill;

    private float lastSpawn = 0;

    private void LoadAllMineralData()
    {
        allMineralData = Resources.LoadAll<MineralData>("Minerals");
        if (allMineralData == null || allMineralData.Length == 0)
            Debug.LogWarning("No MineralData assets found in Resources/Minerals");
    }

    private MineralData ChooseRandomMineral()
    {
        List<MineralData> options = new List<MineralData>();
        foreach (MineralData mineralData in allMineralData)
        {
            if (mineralData.MinimumDepth <= playerDrill.DrillDepth)
            {
                options.Add(mineralData);
            }
        }
        return options[Random.Range(0, options.Count)];
    }

    private void SpawnRandomMineral()
    {
        if (allMineralData == null || allMineralData.Length == 0 || spawner == null || mainCam == null)
            return;

        // choose a random mineral type
        var data = ChooseRandomMineral();

        // determine right end of camera position; choose random Y within range
        float x = Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect);
        float y = Random.Range(ySpawnRange.x, ySpawnRange.y);

        //slight offset to x to ensure they spawn off camera
        Vector3 spawnPos = new(x + 3, y, 0f);
        spawner.SpawnMineral(data, spawnPos);
    }

    void Awake()
    {
        LoadAllMineralData();
        mainCam = Camera.main;
        playerDrill = FindFirstObjectByType<PlayerDrill>();
    }

    void Start()
    {
        //this doesn't work in Awake()
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gameManager.runStartEvent.AddListener(() =>
        {
            lastSpawn = -spawnDistance;
        });
    }

    void Update()
    {
        while (gameManager.inRun && playerDrill.DrillDepth - lastSpawn >= spawnDistance)
        {
            SpawnRandomMineral();
            lastSpawn += spawnDistance;
        }
    }
}