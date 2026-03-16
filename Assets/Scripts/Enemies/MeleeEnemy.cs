using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    private Vector3 targetPosition;

    public override void Initialize(EnemyData data, Vector3 targetPos)
    {
        base.Initialize(data, targetPos);
        targetPosition = targetPos;
        Debug.Log(targetPos);
    }

    //stoping is already taken care of by default by BaseEnemy
    protected override bool ReadyToAttack()
    {
        return transform.position.x >= targetPosition.x;
    }

    protected override void Attack()
    {
        Debug.Log("attack");
        GameManager.instance.PlayerDrill.DrillHealth.TakeDamage(Damage);
    }
}