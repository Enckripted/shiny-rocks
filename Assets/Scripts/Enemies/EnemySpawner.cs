using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    public GameObject SpawnEnemy(EnemyData data, Vector3 position, Vector3 target)
    {
        GameObject nObject = Instantiate(data.EnemyObject, position, Quaternion.identity);

        BaseEnemy enemy = nObject.GetComponent<BaseEnemy>();
        enemy.Initialize(data, target);

        return nObject;
    }
}