using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] private GameObject fireEffect;
    [SerializeField] private Vector3 mousePos;
    [SerializeField] private Vector3 worldMousePos;


    void Awake()
    {
        fireEffect = transform.Find("FireEffect").gameObject;
    }

    void Update()
    {
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
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("hit" + collision.gameObject.name);
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().DealDamage(1);
            }
        }   
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("hit" + collision.gameObject.name);
            if (collision.gameObject.CompareTag("Enemy"))
            {
                StartCoroutine(PlayFireEffect());
                collision.gameObject.GetComponent<Enemy>().DealDamage(5);
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