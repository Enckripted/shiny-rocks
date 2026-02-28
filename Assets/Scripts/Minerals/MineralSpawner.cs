using UnityEngine;

public class MineralSpawner : MonoBehaviour
{
    [SerializeField] private GameObject mineralPrefab;

    public GameObject SpawnMineral(MineralData data, Vector3 position)
    {
        GameObject nObject = Instantiate(mineralPrefab);
        nObject.transform.position = position;
        Mineral nMineral = nObject.GetComponent<Mineral>();
        Debug.Log(data);
        nMineral.Initialize(data);
        return nObject;
    }
}