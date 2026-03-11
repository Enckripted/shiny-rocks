using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Health))]
public class NewMineral : MonoBehaviour
{
    public string MineralName { get; private set; }

    private PlayerDrill playerDrill => GameManager.instance.PlayerDrill;

    private Health health;
    private SpriteRenderer spriteRenderer;
    private AudioSource source;

    [SerializeField] private SpriteRenderer mineralSpriteRenderer;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private AudioClip clip;
    [SerializeField] private NewHealthBar healthBar;

    private bool hasCollidedWithDrill;

    public void Initialize(MineralData data)
    {
        health.MaxHealth = data.MaxHealth;
        MineralName = data.Name;
        mineralSpriteRenderer.color = data.Color;
    }

    public void DestroyMineral()
    {
        transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().color = new(0, 0, 0, 0);
        healthBar.gameObject.transform.Find("Health Bar").gameObject.SetActive(false);
        hitEffect.Play();
        GameManager.instance.AddMineral(MineralName, 1);
        /*
        source.clip = clip;
        source.Play();*/
        StartCoroutine(MineralRemovalDelay());
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();

        health.OnDeath += DestroyMineral;
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