using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<Sprite> sprites;

    public GameObject SpawnEnemy(EnemyData data, Vector3 position, Vector3 target)
    {
        GameObject nObject = Instantiate(data.EnemyObject, position, Quaternion.identity);

        //TODO: keep enemy image completely inside of scriptable objects dont bother with it here
        nObject.GetComponentInChildren<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];

        //temporary, please don't do this later
        if (nObject.GetComponent<MeleeEnemy>() != null)
        {
            MeleeEnemy enemy = nObject.GetComponent<MeleeEnemy>();
            enemy.Initialize(data, target);
        }
        else
        {
            RangedEnemy enemy = nObject.GetComponent<RangedEnemy>();
            enemy.Initialize(data, target);
        }

        return nObject;
    }
}