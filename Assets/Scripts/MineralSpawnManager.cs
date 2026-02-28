using System.Collections;
using UnityEngine;

public class MineralSpawnManager : MonoBehaviour
{
    [SerializeField] private MineralSpawner spawner;
    [SerializeField] private float spawnInterval = 2f;
    private Vector2 ySpawnRange = new(-2,4);

    private MineralData[] allMineralData;
    private Camera mainCam;

    private void LoadAllMineralData()
    {
        allMineralData = Resources.LoadAll<MineralData>("Minerals");
        if (allMineralData == null || allMineralData.Length == 0)
            Debug.LogWarning("No MineralData assets found in Resources/Minerals");
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
        Vector3 spawnPos = new(x+3, y, 0f);
        spawner.SpawnMineral(data, spawnPos);
    }

    void Awake()
    {
        LoadAllMineralData();
        mainCam = Camera.main;
    }

    void Start()
    {
        // start the repeating spawn loop
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnRandomMineral();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}