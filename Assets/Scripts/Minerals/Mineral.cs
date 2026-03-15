using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class Mineral : Damagable
{
    public string MineralName { get; private set; }

    private PlayerDrill playerDrill;
    private SpriteRenderer spriteRenderer;
    private AudioSource source;

    [SerializeField] private SpriteRenderer mineralSpriteRenderer;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private AudioClip clip;

    private bool hasCollidedWithDrill;

    public void Initialize(MineralData data)
    {
        Health = data.MaxHealth;
        MaxHealth = data.MaxHealth;
        MineralName = data.Name;
        mineralSpriteRenderer.color = data.Color;
        UpdateHealthbar();
    }

    public void DestroyMineral()
    {
        transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = new(0, 0, 0, 0);
        transform.Find("Health Bar").gameObject.SetActive(false);
        hitEffect.Play();
        GameManager.instance.AddMineral(MineralName, 1);
        source.clip = clip;
        source.Play();
        StartCoroutine(MineralRemovalDelay());
    }

    void Start()
    {
        playerDrill = FindFirstObjectByType<PlayerDrill>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();
        OnDeathEvent.AddListener(DestroyMineral);
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