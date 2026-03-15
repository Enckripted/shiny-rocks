using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private GameObject lowestSpawnLocation;
    [SerializeField] private GameObject highestSpawnLocation;
    [SerializeField] private GameObject enemyMeleeLocation;

    [SerializeField] private float spawnInterval;
    [SerializeField] private int maxEnemiesAtOnce;

    [SerializeField] private float overlapCheckRadius;
    [SerializeField] private LayerMask overlapLayerMask;

    private EnemyData[] allEnemyData;

    public static int numOfEnemies = 0;

    private Camera mainCamera;

    private void DecrementEnemies()
    {
        numOfEnemies--;
    }

    private void GetAllEnemyData()
    {
        allEnemyData = Resources.LoadAll<EnemyData>("Enemies");
        if (allEnemyData == null || allEnemyData.Length == 0)
            Debug.LogWarning("No EnemyData assets found in Resources/Enemies");
    }

    private Vector3 GetRandEnemySpawnPos()
    {
        Vector3 lowPos = lowestSpawnLocation.transform.position;
        Vector3 highPos = highestSpawnLocation.transform.position;
        return lowPos + new Vector3(0, Random.Range(0f, highPos.y - lowPos.y));
    }

    private bool IsPosValidSpawnPos(Vector3 location)
    {
        Collider2D collider = Physics2D.OverlapCircle(location, overlapCheckRadius, overlapLayerMask);
        return collider == null;
    }

    private Vector3 GetTargetFromStartPos(Vector3 startPos)
    {
        return new Vector3(enemyMeleeLocation.transform.position.x, startPos.y, startPos.z);
    }

    private EnemyData GetRandomEnemyData()
    {
        return allEnemyData[Random.Range(0, allEnemyData.Length)];
    }

    private void AttemptSpawnEnemy()
    {
        Vector3 spawnPos = GetRandEnemySpawnPos();
        if (numOfEnemies >= maxEnemiesAtOnce || !IsPosValidSpawnPos(spawnPos))
        {
            return;
        }

        EnemyData enemyData = GetRandomEnemyData();
        Vector3 targetPos = GetTargetFromStartPos(spawnPos);
        GameObject enemyObj = enemySpawner.SpawnEnemy(enemyData, spawnPos, targetPos);
        BaseEnemy enemy = enemyObj.GetComponent<BaseEnemy>();
        numOfEnemies++;
        //this will also take care of numOfEnemies when everything gets cleaned up on run end
        enemy.OnDeathEvent += DecrementEnemies;
    }

    void Start()
    {
        mainCamera = Camera.main;

        GetAllEnemyData();
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

            AttemptSpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}