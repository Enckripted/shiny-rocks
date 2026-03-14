using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<Sprite> sprites;

    public GameObject SpawnEnemy(EnemyData data, Vector3 position, Vector3 target, Quaternion rotation)
    {
        GameObject nObject = Instantiate(data.EnemyObject);
        nObject.transform.position = position;
        nObject.transform.rotation = rotation;
        if (nObject.transform.rotation != Quaternion.identity)
        {
            Vector3 localScale = nObject.transform.localScale;
            nObject.transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);

            if (nObject.GetComponentInChildren<HealthBar>() != null)
            {
                Transform healthTransform = nObject.GetComponentInChildren<HealthBar>().gameObject.transform;
                healthTransform.localScale = new Vector3(healthTransform.localScale.x, healthTransform.localScale.y * -1, healthTransform.localScale.z);
                healthTransform.localPosition = new Vector3(0, 0.6f, 0);
            }
        }
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