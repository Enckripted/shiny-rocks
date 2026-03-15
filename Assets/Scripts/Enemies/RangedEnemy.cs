using System;
using UnityEngine;

//(mostly) ai gen
public class RangedEnemy : BaseEnemy
{
    [SerializeField] private GameObject bulletPrefab; // Assign in inspector
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float distanceFromDrillBase = 5f;
    [SerializeField] private float distanceFromDrillRange = 2f;

    private float finalDrillDistance;

    private Vector3 targetPosition;

    public override void Initialize(EnemyData data, Vector3 target)
    {
        base.Initialize(data, target);
        targetPosition = target;
        finalDrillDistance = distanceFromDrillBase + distanceFromDrillRange * UnityEngine.Random.Range(-1f, 1f);
    }

    protected override bool ReadyToAttack()
    {
        return transform.position.x >= targetPosition.x - distanceFromDrillBase;
    }

    protected override void Attack()
    {
        // Shoot a bullet instead of melee attack
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        RangedBullet rangedBullet = bullet.GetComponent<RangedBullet>();
        rangedBullet.Initialize(Damage, bulletSpeed, GameManager.instance.PlayerDrill.transform.position);
    }

    protected override void OnAwake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
}