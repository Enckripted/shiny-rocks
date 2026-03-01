using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponBase : MonoBehaviour
{

    [SerializeField] private PlayerDrill playerDrill;
    [SerializeField] private GameObject fireEffect;
    [SerializeField] private Vector3 mousePos;
    [SerializeField] private Vector3 worldMousePos;
    [SerializeField] private float shotCooldownTimer;
    [SerializeField] private float shotCooldown = 0.5f;


    void Awake()
    {
        fireEffect = transform.Find("FireEffect").gameObject;
        playerDrill = FindFirstObjectByType<PlayerDrill>();
    }

    void Update()
    {
        if(shotCooldownTimer > 0)
        {
            shotCooldownTimer -= Time.deltaTime;
            transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = new Color(.25f, .25f, .25f);
        }
        else
        {
            transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        }

        GameObject weaponCollider = transform.Find("WeaponCollider").gameObject;
        mousePos = Mouse.current.position.ReadValue();
        worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(
            mousePos.x,
            mousePos.y,
            Camera.main.nearClipPlane
        ));
        weaponCollider.transform.position = worldMousePos;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && shotCooldownTimer <= 0)
        {
            Debug.Log("hit" + collision.gameObject.name);
            if (collision.gameObject.CompareTag("Enemy"))
            {
                StartCoroutine(PlayFireEffect());
                collision.gameObject.GetComponent<Enemy>().DealDamage(playerDrill.WeaponDamage);
                shotCooldownTimer = shotCooldown;
            }
        }   
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && shotCooldownTimer <= 0)
        {
            Debug.Log("hit" + collision.gameObject.name);
            if (collision.gameObject.CompareTag("Enemy"))
            {
                StartCoroutine(PlayFireEffect());
                collision.gameObject.GetComponent<Enemy>().DealDamage(playerDrill.WeaponDamage);
                shotCooldownTimer = shotCooldown;
            }
        }   
    }

    IEnumerator PlayFireEffect()
    {
        fireEffect.GetComponent<SpriteRenderer>().color = new Color(1,0,0,1);
        yield return new WaitForSeconds(.2f);
        fireEffect.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
    }

}