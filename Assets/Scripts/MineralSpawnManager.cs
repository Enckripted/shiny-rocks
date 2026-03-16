using System;
using System.Collections.Generic;
using UnityEngine;

public class MineralSpawnManager : MonoBehaviour
{
    [SerializeField] private MineralSpawner spawner;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float baseSpawnDistance = 0.5f;
    [SerializeField] private float spawnDistanceDecrement = 0.05f;
    [SerializeField] private float decrementDepthAmount = 10f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float intersectionCheckRadius;
    private float spawnDistance => Math.Max(baseSpawnDistance - spawnDistanceDecrement * (PlayerDrill.instance.DrillDepth / decrementDepthAmount), 0.05f);
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
        return options[UnityEngine.Random.Range(0, options.Count)];
    }

    private bool IsPosValidSpawnPos(Vector3 location)
    {
        Collider2D collider = Physics2D.OverlapCircle(location, intersectionCheckRadius, layerMask);
        if (collider) Debug.Log(collider);
        return collider == null;
    }

    private void AttemptSpawnRandomMineral()
    {
        if (allMineralData == null || allMineralData.Length == 0 || spawner == null || mainCam == null)
            return;

        // choose a random mineral type
        var data = ChooseRandomMineral();

        // determine right end of camera position; choose random Y within range
        float x = Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect) + 3;
        float y = UnityEngine.Random.Range(ySpawnRange.x, ySpawnRange.y);
        Debug.Log(new Vector3(x, y));
        if (!IsPosValidSpawnPos(new Vector3(x, y)))
            return;

        //slight offset to x to ensure they spawn off camera
        Vector3 spawnPos = new(x, y, 0f);
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

            AttemptSpawnRandomMineral();
            lastSpawn += spawnDistance;
        }
    }
}