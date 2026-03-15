using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Health))]
public class Mineral : MonoBehaviour
{
    public string MineralName { get; private set; }

    private PlayerDrill playerDrill;
    private SpriteRenderer spriteRenderer;
    private AudioSource source;
    private Health health;

    [SerializeField] private SpriteRenderer mineralSpriteRenderer;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private AudioClip clip;

    public void Initialize(MineralData data)
    {
        health.MaxHealth = data.MaxHealth;
        MineralName = data.Name;
        mineralSpriteRenderer.color = data.Color;
    }

    public void DestroyMineral()
    {
        mineralSpriteRenderer.color = new(0, 0, 0, 0);
        health.gameObject.SetActive(false);
        hitEffect.Play();
        GameManager.instance.AddMineral(MineralName, 1);
        //source.clip = clip;
        //source.Play();
        StartCoroutine(MineralRemovalDelay());
    }

    void Awake()
    {
        playerDrill = FindFirstObjectByType<PlayerDrill>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();
        health = GetComponent<Health>();

        health.OnDeath += DestroyMineral;
    }

    void OnDestroy()
    {
        health.OnDeath -= DestroyMineral;
    }

    //ai gen movement + june edits
    void Update()
    {
        if (!GameManager.instance.inRun)
        {
            Destroy(gameObject);
            return;
        }

        // move left according to the player's drill speed (assume PlayerController always exists)
        if (playerDrill.IsMoving)
        {
            transform.Translate(Vector3.left * (float)playerDrill.DrillSpeed * Time.deltaTime, Space.World);
        }

        // destroy once it is no longer visible by any camera
        if (spriteRenderer != null && !spriteRenderer.isVisible && transform.position.x < 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator MineralRemovalDelay()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}