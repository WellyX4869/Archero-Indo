using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : State
{
    float elapsedTime = 0f;
    public override void EnterState(EnemyController enemy)
    {
        enemy.navAgent.isStopped = true;
        enemy.navAgent.stoppingDistance = 0f;
        enemy.animator.SetBool("Hit", true);

        if (!enemy.isRanged)
        {
            enemy.transform.LookAt(enemy.player.transform.position);
        }
    }

    public override void Update(EnemyController enemy)
    {
        elapsedTime += Time.deltaTime;
        enemy.animator.SetBool("Hit", false);

        if (enemy.isRanged && elapsedTime >= enemy.hitLength)
        {
            enemy.TransitionToState(new AttackState());
        }
        else if(!enemy.isRanged)
        {
            enemy.TransitionToState(new AttackState());
        }
    }
}
