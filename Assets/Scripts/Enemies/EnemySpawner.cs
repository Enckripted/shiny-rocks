using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    public GameObject SpawnEnemy(EnemyData data, Vector3 position)
    {
        GameObject nObject = Instantiate(enemyPrefab);
        nObject.transform.position = position;

        Enemy enemy = nObject.GetComponent<Enemy>();
        //TODO: initialize enemy
        return nObject;
    }
}