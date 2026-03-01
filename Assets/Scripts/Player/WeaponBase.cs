using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseCircleCast : MonoBehaviour
{
    [SerializeField] private GameObject fireEffect;
    [SerializeField] private float radius = 0.5f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Vector3 mousePos;
    [SerializeField] private Vector3 worldMousePos;

    void Awake()
    {
        fireEffect = transform.Find("FireEffect").gameObject;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            CircleCast();
            StartCoroutine(PlayFireEffect());
        }
    }

    void CircleCast()
    {
        // Convert mouse position to world position
        mousePos = Mouse.current.position.ReadValue();
        worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(
            mousePos.x,
            mousePos.y,
            Camera.main.nearClipPlane
        ));
        worldMousePos.z = 0f;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            worldMousePos,
            radius,
            Vector2.zero,
            0f,
            layerMask
        );

        Debug.Log(hits);
    }

    private void OnDrawGizmos()
    {
        if (Camera.main == null) return;

        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePos);
        worldMousePosition.z = 0f;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(worldMousePosition, radius);
    }

    IEnumerator PlayFireEffect()
    {
        fireEffect.GetComponent<SpriteRenderer>().color = new Color(1,0,0,1);
        yield return new WaitForSeconds(.2f);
        fireEffect.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
    }
}