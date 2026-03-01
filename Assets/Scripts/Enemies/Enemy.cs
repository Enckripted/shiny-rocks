using UnityEngine;

public class Enemy : Damagable
{
    public float Speed { get; private set; }
    public float Damage { get; private set; }
    public float AttackSpeed { get; private set; }

    private Vector3 targetPosition;
    private Rigidbody2D rb;
    private PlayerDrill playerDrill;

    private float lastAttack = 0;

    public void Initialize(EnemyData data, Vector3 target)
    {
        Health = data.MaxHealth;
        MaxHealth = data.MaxHealth;
        Speed = data.Speed;
        Damage = data.Damage;
        AttackSpeed = data.AttackSpeed;
        UpdateHealthbar();

        targetPosition = target;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerDrill = FindFirstObjectByType<PlayerDrill>();
    }

    void Update()
    {
        if (Health <= 0 || !GameManager.instance.inRun)
        {
            Destroy(gameObject);
            return;
        }

        if (transform.position.x >= targetPosition.x)
        {
            transform.position = targetPosition;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;

            //attack
            if (Time.fixedTime - lastAttack >= AttackSpeed)
            {
                playerDrill.DealDamage(Damage);
                lastAttack = Time.fixedTime;
            }

            return;
        }
        rb.linearVelocity = Vector2.right * Speed;
    }
}