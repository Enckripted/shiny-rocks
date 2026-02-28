using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Mineral : Damagable
{
    private PlayerDrill playerDrill;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private ParticleSystem hitEffect;

    private bool hasCollidedWithDrill;
    private bool isRemoveRunning;

    public void Initialize(MineralData data)
    {
        Health = data.MaxHealth;
        MaxHealth = data.MaxHealth;
    }

    void Awake()
    {
        playerDrill = FindFirstObjectByType<PlayerDrill>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //ai gen movement + june edits
    void Update()
    {
        // move left according to the player's drill speed (assume PlayerController always exists)
        if (!hasCollidedWithDrill)
        {
            transform.Translate(Vector3.left * playerDrill.DrillSpeed * Time.deltaTime, Space.World);
        }

        // destroy once it is no longer visible by any camera
        if (spriteRenderer != null && !spriteRenderer.isVisible && transform.position.x < 0)
        {
            Destroy(gameObject);
        }

        if (Health <= 0)
        {
            StartCoroutine(MineralRemove());
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Drill"))
        {
            if (!hasCollidedWithDrill)
            {
                StartCoroutine(DrainHP());
                hasCollidedWithDrill = true;
            }
        }
    }

    private IEnumerator DrainHP()
    {
        //deal damage based on player's drill damage, time interval based on drillSpeed * 0.1
        DealDamage(playerDrill.DrillDamage);
        hitEffect.Play();
        yield return new WaitForSeconds(2 * (playerDrill.DrillSpeed * 0.1f));
    }

    private IEnumerator MineralRemove()
    {
        if (!isRemoveRunning)
        {
            //set mineral sprite's alpha to 0, play hitEffect
            transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = new(0, 0, 0, 0);
            transform.Find("Health Bar").gameObject.SetActive(false);
            hitEffect.Play();
            isRemoveRunning = true;
        }
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

}