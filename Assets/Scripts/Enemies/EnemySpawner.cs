using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<Sprite> sprites;

    public GameObject SpawnEnemy(EnemyData data, Vector3 position, Vector3 target, Quaternion rotation)
    {
        GameObject nObject = Instantiate(enemyPrefab);
        nObject.transform.position = position;
        nObject.transform.rotation = rotation;
        if (nObject.transform.rotation != Quaternion.identity)
        {
            Vector3 localScale = nObject.transform.localScale;
            nObject.transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);

            Transform healthTransform = nObject.GetComponentInChildren<HealthBar>().gameObject.transform;
            healthTransform.localScale = new Vector3(healthTransform.localScale.x, healthTransform.localScale.y * -1, healthTransform.localScale.z);
            healthTransform.localPosition = new Vector3(0, 0.6f, 0);
        }
        nObject.GetComponentInChildren<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];

        Enemy enemy = nObject.GetComponent<Enemy>();
        enemy.Initialize(data, target);
        return nObject;
    }
}