using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class MeleeEnemy : MonoBehaviour
{
    public float Speed { get; private set; }
    public float Damage { get; private set; }
    public float AttackSpeed { get; private set; }

    private Vector3 targetPosition;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private Health health;
    private float lastAttack = 0;

    public void Initialize(EnemyData data, Vector3 target)
    {
        float scaleLevel = (float)Math.Floor(GameManager.instance.PlayerDrill.DrillDepth / 10);
        float maxHealth = data.MaxHealth * (float)Math.Pow(data.HealthScaleMult, scaleLevel);

        health.MaxHealth = maxHealth;
        Speed = data.Speed + data.SpeedScaleAdd * scaleLevel;
        Damage = data.Damage + (float)Math.Pow(data.DamageScaleMult, scaleLevel);
        AttackSpeed = data.AttackSpeed - data.AttackSpeedScaleAdd * scaleLevel;

        targetPosition = target;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        health = GetComponent<Health>();
    }

    void Update()
    {
        if (health.CurrentHealth <= 0 || !GameManager.instance.inRun || !spriteRenderer.isVisible)
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
                GameManager.instance.PlayerDrill.DealDamage(Damage);
                lastAttack = Time.fixedTime;
            }

            return;
        }
        //move slower if the drill is currently moving forward
        rb.linearVelocity = Vector2.right * Speed + (GameManager.instance.PlayerDrill.IsMoving ? Vector2.left * (float)GameManager.instance.PlayerDrill.DrillSpeed : Vector2.zero);
    }
}