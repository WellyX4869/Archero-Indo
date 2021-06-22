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
            enemy.ShootDangerMarker1();
        }
    }

    public override void Update(EnemyController enemy)
    {
        elapsedTime += Time.deltaTime;

        // FOR MELEE
        if (!enemy.isRanged && enemy.firstAttack)
        {
            enemy.TransitionToState(new HitState());
            enemy.firstAttack = false;
            elapsedTime = 0f;
        }

        // FOR RANGED
        if (enemy.isRanged && enemy.rangedType == 2 && elapsedTime < enemy.attackCooldown)
        {
            enemy.ShootDangerMarker2();
        }

        if (!enemy.IsPlayerWithinAttackRange())
        {
            enemy.animator.SetBool("Attack", false);
            if(enemy.isRanged)
                enemy.DangerMarkerDeactivate();
            else enemy.firstAttack = true;
            enemy.TransitionToState(new ChaseState());
        }

        if(elapsedTime >= enemy.attackCooldown)
        {
            if(enemy.isRanged)
                enemy.DangerMarkerDeactivate();
            
            if(elapsedTime >= enemy.attackCooldown + enemy.exitTime)
            {
                enemy.TransitionToState(new HitState());
                elapsedTime = 0f; 
            }
        }
    }
}
