using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerEnemy : BaseEnemy
{
    [SerializeField] private float healRadius = 1f;
    [SerializeField] private float xDisplacementRange = 10f;
    [SerializeField] private float yDisplacementRange = 1f;
    [SerializeField] private float distanceForSuccesfulMove = 0.1f;
    [SerializeField] private GameObject healBubblePrefab;

    [SerializeField] private LayerMask layerMask;

    private Vector3 targetPosition;
    private Vector3 randPosition;

    private Vector3 GetNewRandPos()
    {
        Vector3 pos = targetPosition + new Vector3(
            -Random.Range(0, xDisplacementRange), Random.Range(-yDisplacementRange, yDisplacementRange), 0
        );
        return pos;
    }

    private void DoBubbleAnimation()
    {
        GameObject healBubbleObj = Instantiate(healBubblePrefab, transform.position, Quaternion.identity);
        healBubbleObj.transform.localScale *= healRadius;
    }

    private List<Health> GetNearbyEnemyHealths()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, healRadius, layerMask);
        List<Health> result = new();

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.GetComponent<BaseEnemy>() == null || collider.gameObject == gameObject)
            {
                continue;
            }

            Health health = collider.gameObject.GetComponent<Health>();
            result.Add(health);
        }

        return result;
    }

    public override void Initialize(EnemyData data, Vector3 targetPos)
    {
        base.Initialize(data, targetPos);
        targetPosition = targetPos;
        randPosition = GetNewRandPos();
    }

    protected override bool ReadyToAttack()
    {
        return true;
    }

    protected override void Attack()
    {
        List<Health> healths = GetNearbyEnemyHealths();
        if (healths.Count == 0)
            return;

        foreach (Health health in healths)
        {
            health.Heal(Damage);
        }
        DoBubbleAnimation();
    }

    protected override void DoMovement()
    {
        rb.linearVelocity = (randPosition - transform.position).normalized * Speed;
        //TODO: maybe have this check scale with speed?
        if ((randPosition - transform.position).magnitude < distanceForSuccesfulMove)
        {
            randPosition = GetNewRandPos();
        }
    }

    protected override void DoAttackReady()
    {
        DoMovement();
    }
}