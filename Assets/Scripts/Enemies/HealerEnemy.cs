using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerEnemy : BaseEnemy
{
    [SerializeField] private float healRadius = 1f;
    [SerializeField] private float xDisplacementRange = 10f;
    [SerializeField] private float yDisplacementRange = 1f;
    [SerializeField] private float distanceForSuccesfulMove = 0.1f;

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
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, healRadius, layerMask);

        int hits = 0;
        foreach (Collider2D collider in results)
        {
            if (collider.gameObject.GetComponent<BaseEnemy>() == null)
            {
                continue;
            }

            Health health = collider.gameObject.GetComponent<Health>();
            health.Heal(health.MaxHealth / 5);
            hits++;
        }

        if (hits > 0)
        {
            Debug.Log($"casted heal on {hits} targets");
            //StartCoroutine(HealBubbleAnimation());
        }
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

    /*
    IEnumerator HealBubbleAnimation()
    {
        //yield return null;
    }*/
}