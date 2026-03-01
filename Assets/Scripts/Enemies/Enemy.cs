using System;
using UnityEngine;

public class Enemy : Damagable
{
    public float Speed { get; private set; }
    public float Damage { get; private set; }
    public float AttackSpeed { get; private set; }

    private Vector3 targetPosition;
    private Rigidbody2D rb;
    private PlayerDrill playerDrill;
    private SpriteRenderer spriteRenderer;

    private float lastAttack = 0;

    public void Initialize(EnemyData data, Vector3 target)
    {
        float scaleLevel = (float)Math.Floor(playerDrill.DrillDepth / 10);
        float maxHealth = data.MaxHealth * (float)Math.Pow(data.HealthScaleMult, scaleLevel);

        Health = maxHealth;
        MaxHealth = maxHealth;
        Speed = data.Speed + data.SpeedScaleAdd * scaleLevel;
        Damage = data.Damage + (float)Math.Pow(data.DamageScaleMult, scaleLevel);
        AttackSpeed = data.AttackSpeed - data.AttackSpeedScaleAdd * scaleLevel;
        UpdateHealthbar();

        targetPosition = target;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerDrill = FindFirstObjectByType<PlayerDrill>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Health <= 0 || !GameManager.instance.inRun || !spriteRenderer.isVisible)
        {
            EnemySpawnManager.numOfEnemies--;
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
        //move slower if the drill is currently moving forward
        rb.linearVelocity = Vector2.right * Speed + (playerDrill.IsMoving ? Vector2.left * playerDrill.DrillSpeed : Vector2.zero);
    }
}