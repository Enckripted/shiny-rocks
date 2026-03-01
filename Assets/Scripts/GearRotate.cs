using UnityEngine;

public class GearRotate : MonoBehaviour
{

    [SerializeField] private float rotateSpeed;

    void Update()
    {
        if (FindFirstObjectByType<PlayerDrill>().IsMoving)
        {    
            gameObject.transform.Rotate(new Vector3(0,0,1*rotateSpeed));
        }
    }
}
