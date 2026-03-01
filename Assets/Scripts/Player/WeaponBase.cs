using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] private GameObject fireEffect;
    [SerializeField] private float radius = 0.5f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Vector3 mousePos;
    [SerializeField] private Vector3 worldMousePos;
    [SerializeField] public int WeaponDamage; //public for shop upgrades
    [SerializeField] public float weaponCooldown;
    [SerializeField] public float weaponCooldownTimer;

    void Awake()
    {
        fireEffect = transform.Find("FireEffect").gameObject;
    }

    void Update()
    {
        mousePos = Mouse.current.position.ReadValue();
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            CircleCast();
            StartCoroutine(PlayFireEffect());
        }
        // Convert mouse position to world position
        worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(
            mousePos.x,
            mousePos.y,
            Camera.main.nearClipPlane
        ));
        worldMousePos.z = 0f;

        //set crosshair position
        transform.Find("WeaponCollider").transform.position = worldMousePos;
    }

    void CircleCast()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            worldMousePos,
            radius,
            Vector2.zero,
            0f,
            layerMask
        );

        for(var i = 0; i < hits.Length; i++)
        {
            hits[i].collider.gameObject.GetComponent<Enemy>().DealDamage(WeaponDamage);
        }
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