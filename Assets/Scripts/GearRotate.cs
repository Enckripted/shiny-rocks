using UnityEngine;

public class GearRotate : MonoBehaviour
{

    [SerializeField] private float rotateSpeed;

    private PlayerDrill playerDrill;

    void Start()
    {
        playerDrill = FindFirstObjectByType<PlayerDrill>();
    }

    void Update()
    {
        if (GameManager.instance.inRun && FindFirstObjectByType<PlayerDrill>().IsMoving)
        {
            gameObject.transform.Rotate(new Vector3(0, 0, 1 * rotateSpeed * (float)(playerDrill.DrillStats.DrillSpeed / 4)));
        }
    }
}
