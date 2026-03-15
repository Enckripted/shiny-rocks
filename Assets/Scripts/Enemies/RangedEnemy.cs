using System;
using UnityEngine;

//(mostly) ai gen
public class RangedEnemy : BaseEnemy
{
    [SerializeField] private GameObject bulletPrefab; // Assign in inspector
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float distanceFromDrill = 5f;
    private Vector3 targetPosition;

    public void Initialize(EnemyData data, Vector3 target)
    {
        base.Initialize(data);
        targetPosition = target;
    }

    protected override bool ReadyToAttack()
    {
        return transform.position.x >= targetPosition.x - distanceFromDrill;
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