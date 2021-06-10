using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    float elapsedTime = 0f;

    public override void EnterState(EnemyController enemy)
    {
        enemy.navAgent.isStopped = true;
        enemy.navAgent.stoppingDistance = 0f;
        enemy.animator.SetBool("Attack", true);
        if (enemy.isRanged && enemy.rangedType == 1)
        {
            enemy.ShootDangerMarker();
        }
    }

    public override void Update(EnemyController enemy)
    {
        elapsedTime += Time.deltaTime;
        if (enemy.isRanged && enemy.rangedType == 2)
        {
            enemy.ShootDangerMarker2();
        }

        if (!enemy.IsPlayerWithinAttackRange())
        {
            enemy.animator.SetBool("Attack", false);
            enemy.DangerMarkerDeactivate();
            enemy.TransitionToState(new ChaseState());
        }

        if(elapsedTime >= enemy.attackCooldown)
        {
            enemy.DangerMarkerDeactivate();
            enemy.TransitionToState(new HitState());
            elapsedTime = 0f;
        }
    }

    //private void RangedUpdate(EnemyController enemy)
    //{
    //    // CREATE DANGER LINE
    //    enemy.ShootDangerMarker();
    //}
}
