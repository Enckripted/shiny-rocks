using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private GameObject lanesObject;

    [SerializeField] private float spawnInterval;

    private EnemyData[] allEnemyData;
    private List<Vector3> lanePositions;

    private Camera mainCamera;

    private void GetAllEnemyData()
    {
        allEnemyData = Resources.LoadAll<EnemyData>("Enemies");
        if (allEnemyData == null || allEnemyData.Length == 0)
            Debug.LogWarning("No EnemyData assets found in Resources/Enemies");
    }

    private void CreateLanesList()
    {
        lanePositions = new List<Vector3>();
        foreach (Transform laneObj in lanesObject.GetComponentsInChildren<Transform>())
        {
            lanePositions.Add(laneObj.position);
        }
        lanePositions.RemoveAt(0); //getcomponentsinchildren counts the object itself as well?
    }

    private EnemyData GetRandomEnemyData()
    {
        return allEnemyData[Random.Range(0, allEnemyData.Count<EnemyData>() - 1)];
    }

    private void SpawnEnemyInLane(int lane)
    {
        EnemyData enemyData = GetRandomEnemyData();
        Vector3 lanePosition = lanePositions[lane];
        //Debug.Log(lane);
        //Debug.Log(lanePosition);
        float leftXPosition = mainCamera.transform.position.x - (mainCamera.aspect * mainCamera.orthographicSize);
        enemySpawner.SpawnEnemy(enemyData, new(leftXPosition, lanePosition.y, lanePosition.z), lanePosition);
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
            SpawnEnemyInRandLane();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}