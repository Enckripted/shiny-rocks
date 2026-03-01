using UnityEngine;

public class Enemy : Damagable
{
    public float Speed { get; private set; }
    public float Damage { get; private set; }

    private Vector3 targetPosition;
    private Rigidbody2D rb;

    public void Initialize(EnemyData data, Vector3 target)
    {
        Health = data.MaxHealth;
        MaxHealth = data.MaxHealth;
        Speed = data.Speed;
        Damage = data.Damage;
        UpdateHealthbar();

        targetPosition = target;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!GameManager.instance.inRun)
        {
            Destroy(gameObject);
            return;
        }

        if (transform.position.x >= targetPosition.x)
        {
            transform.position = targetPosition;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            return;
        }
        rb.linearVelocity = Vector2.right * Speed;

        if(Health <= 0)
        {
            Destroy(gameObject);
        }

    }
}