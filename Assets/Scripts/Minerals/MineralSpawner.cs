using System.Collections.Generic;
using UnityEngine;

public class MineralSpawner : MonoBehaviour
{
    [SerializeField] private GameObject mineralPrefab;
    [SerializeField] private List<Sprite> shapeList;

    public GameObject SpawnMineral(MineralData data, Vector3 position)
    {
        GameObject nObject = Instantiate(mineralPrefab);

        int randInt = Random.Range(1,5);
        nObject.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = shapeList[randInt-1];

        nObject.transform.position = position;
        Mineral nMineral = nObject.GetComponent<Mineral>();
        nMineral.Initialize(data);
        return nObject;
    }
}