using System.Collections;
using UnityEngine;

public class MineralSpawnManager : MonoBehaviour
{
    [SerializeField] private MineralSpawner spawner;
    [SerializeField] private float spawnInterval = 2f;

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

        // determine world position at left edge; randomised vertically between top/bottom
        float y = mainCam.ViewportToWorldPoint(new Vector3(0f, Random.value, mainCam.nearClipPlane)).y;
        float x = mainCam.ViewportToWorldPoint(new Vector3(0f, 0f, mainCam.nearClipPlane)).x;
        Vector3 spawnPos = new Vector3(x, y, 0f);

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