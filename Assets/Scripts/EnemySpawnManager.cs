using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private GameObject lanesObject;

    [SerializeField] private float spawnInterval;
    [SerializeField] private int maxEnemiesAtOnce;

    private EnemyData[] allEnemyData;
    private List<Transform> lanePositions;

    public static int numOfEnemies;

    private Camera mainCamera;

    private void GetAllEnemyData()
    {
        allEnemyData = Resources.LoadAll<EnemyData>("Enemies");
        if (allEnemyData == null || allEnemyData.Length == 0)
            Debug.LogWarning("No EnemyData assets found in Resources/Enemies");
    }

    private void CreateLanesList()
    {
        lanePositions = new List<Transform>();
        foreach (Transform laneObj in lanesObject.GetComponentsInChildren<Transform>())
        {
            lanePositions.Add(laneObj);
        }
        lanePositions.RemoveAt(0); //getcomponentsinchildren counts the object itself as well?
    }

    private EnemyData GetRandomEnemyData()
    {
        return allEnemyData[Random.Range(0, allEnemyData.Length)];
    }

    private void SpawnEnemyInLane(int lane)
    {
        EnemyData enemyData = GetRandomEnemyData();
        Transform laneTransform = lanePositions[lane];
        Vector3 lanePosition = laneTransform.position;
        float leftXPosition = mainCamera.transform.position.x - (mainCamera.aspect * mainCamera.orthographicSize);
        enemySpawner.SpawnEnemy(enemyData, new(leftXPosition, lanePosition.y, lanePosition.z), lanePosition, laneTransform.rotation);
        numOfEnemies++;
    }

    private void SpawnEnemyInRandLane()
    {
        int index = Random.Range(0, lanePositions.Count);
        SpawnEnemyInLane(index);
    }

    void Start()
    {
        mainCamera = Camera.main;

        GetAllEnemyData();
        CreateLanesList();
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (!GameManager.instance.inRun)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }

            if (numOfEnemies < maxEnemiesAtOnce)
                SpawnEnemyInRandLane();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}