using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    public GameObject SpawnEnemy(EnemyData data, Vector3 position, Vector3 target)
    {
        //Debug.Log(target);
        GameObject nObject = Instantiate(enemyPrefab);
        nObject.transform.position = position;
        Enemy enemy = nObject.GetComponent<Enemy>();
        enemy.Initialize(data, target);
        return nObject;
    }
}