using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] private GameObject fireEffect;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Vector3 mousePos;
    [SerializeField] private Vector3 worldMousePos;
    [SerializeField] public double weaponCooldownTimer;

    [SerializeField] private Sprite weaponReady;
    [SerializeField] private Sprite weaponOnCooldown;

    [SerializeField] private Vector3 rotateOffset;

    void Awake()
    {
        fireEffect = transform.Find("FireEffect").gameObject;
    }

    void Update()
    {

        if(weaponCooldownTimer > 0)
        {
            weaponCooldownTimer -= Time.deltaTime;
            transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = weaponOnCooldown;
        }
        else
        {
            transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = weaponReady;
        }

        mousePos = Mouse.current.position.ReadValue();
        // Convert mouse position to world position
        worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(
            mousePos.x,
            mousePos.y,
            Camera.main.nearClipPlane
        ));

        //rotate weapon to face mouse pointer
        Vector2 direction = worldMousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;    
        

        // Sprite points left, so offset by 180
        angle += 180f;
        

        // Clamp the angle relative to sprite’s left direction
        float clampedAngle = angle;//Mathf.Clamp(angle-90, 90f, 270f);
        if(angle < 280 && angle > 180) clampedAngle = 280;
        else if(angle > 80 && angle < 180) clampedAngle = 80;
        transform.rotation = Quaternion.Euler(0f, 0f, clampedAngle);

        if (Mouse.current.leftButton.wasPressedThisFrame && weaponCooldownTimer <= 0)
        {
            CircleCast();
            StartCoroutine(PlayFireEffect());
        }
        
        
        worldMousePos.z = 0f;

        //set crosshair position
        transform.Find("WeaponCollider").transform.position = worldMousePos;
    }

    void CircleCast()
    {
        if(weaponCooldownTimer <= 0)
        {
            weaponCooldownTimer = PlayerDrill.instance.WeaponCooldown;
        } else
        {
            return;
        }

        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            worldMousePos,
            (float)PlayerDrill.instance.WeaponRadius,
            Vector2.zero,
            0f,
            layerMask
        );

        for(var i = 0; i < hits.Length; i++)
        {
            hits[i].collider.gameObject.GetComponent<Enemy>().DealDamage((float)PlayerDrill.instance.WeaponDamage);
        }
    }

    IEnumerator PlayFireEffect()
    {
        fireEffect.GetComponent<SpriteRenderer>().color = new Color(1,0,0,1);
        yield return new WaitForSeconds(.2f);
        fireEffect.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
    }
}