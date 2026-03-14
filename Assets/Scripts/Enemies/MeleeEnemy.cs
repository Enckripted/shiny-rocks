using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    private Vector3 targetPosition;

    public void Initialize(EnemyData data, Vector3 targetPos)
    {
        base.Initialize(data);
        targetPosition = targetPos;
    }

    //stoping is already taken care of by default by BaseEnemy
    protected override bool ReadyToAttack()
    {
        return transform.position.x >= targetPosition.x;
    }

    protected override void Attack()
    {
        GameManager.instance.PlayerDrill.DealDamage(Damage);
    }
}