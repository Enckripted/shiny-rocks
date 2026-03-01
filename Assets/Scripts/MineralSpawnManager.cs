using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralSpawnManager : MonoBehaviour
{
    [SerializeField] private MineralSpawner spawner;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float spawnInterval = 2f;
    private Vector2 ySpawnRange = new(-2, 4);

    private MineralData[] allMineralData;
    private Camera mainCam;
    private PlayerDrill playerDrill;

    private bool isSpawning = false;

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
        var data = allMineralData[Random.Range(0, allMineralData.Length)];

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
    }

    void Update()
    {
        //ensures that coroutine will not start again once started
        //and will not stop again once stopped
        if (gameManager.inRun && !isSpawning)
        {
            StartCoroutine(SpawnLoop());
            isSpawning = true;

        }
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (!gameManager.inRun)
            {
                isSpawning = false;
                yield break;
            }

            SpawnRandomMineral();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}