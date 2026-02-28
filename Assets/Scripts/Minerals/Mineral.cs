using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Mineral : Damagable
{
    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;

    public void Initialize(MineralData data)
    {
        Health = data.MaxHealth;
        MaxHealth = data.MaxHealth;
    }

    void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //ai gen movement
    void Update()
    {
        // move left according to the player's drill speed (assume PlayerController always exists)
        transform.Translate(Vector3.right * playerController.DrillSpeed * Time.deltaTime, Space.World);

        // destroy once it is no longer visible by any camera
        if (spriteRenderer != null && !spriteRenderer.isVisible)
        {
            Destroy(gameObject);
        }
    }
}