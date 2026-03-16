using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public abstract class BaseEnemy : MonoBehaviour
{
    public float Speed { get; protected set; }
    public float Damage { get; protected set; }
    public float AttackSpeed { get; protected set; }

    public event Action OnDeathEvent;

    protected Rigidbody2D rb;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    protected Health health;
    protected float lastAttack = 0;

    protected virtual void StopMoving()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    protected virtual void KillEnemy()
    {
        OnDeathEvent?.Invoke();
        Destroy(gameObject);
    }

    public virtual void Initialize(EnemyData data, Vector3 targetPosition)
    {
        float scaleLevel = (float)Math.Floor(GameManager.instance.PlayerDrill.DrillDepth / 10);
        float maxHealth = data.MaxHealth * (float)Math.Pow(data.HealthScaleMult, scaleLevel);

        health.SetMaxHealth(maxHealth);
        Speed = data.Speed + data.SpeedScaleAdd * scaleLevel;
        Damage = data.Damage + (float)Math.Pow(data.DamageScaleMult, scaleLevel);
        AttackSpeed = data.AttackSpeed - data.AttackSpeedScaleAdd * scaleLevel;

        spriteRenderer.sprite = data.Sprite[UnityEngine.Random.Range(0, data.Sprite.Count)];
    }

    protected abstract bool ReadyToAttack();
    protected abstract void Attack();

    protected virtual void DoMovement()
    {
        rb.linearVelocity = Vector2.right * Speed;
    }

    protected virtual void DoAttackReady()
    {
        StopMoving();
    }

    protected virtual void OnDeath() { }
    protected virtual void OnAwake() { }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        health.OnDeath += KillEnemy;
        OnAwake();
    }

    void Update()
    {
        if (!GameManager.instance.inRun)
        {
            KillEnemy();
        }

        if (ReadyToAttack())
        {
            DoAttackReady();
            if (Time.fixedTime - lastAttack >= AttackSpeed)
            {
                Attack();
                lastAttack = Time.fixedTime;
            }
        }
        else
        {
            DoMovement();
        }
    }
}