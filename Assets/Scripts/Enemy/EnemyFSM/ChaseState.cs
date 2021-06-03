using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public override void EnterState(EnemyController enemy)
    {
        enemy.navAgent.isStopped = false;
        enemy.animator.SetBool("Chase", true);
    }

    public override void Update(EnemyController enemy)
    {
        if(enemy.player.activeSelf == false)
        {
            return;
        }

        enemy.navAgent.SetDestination(enemy.player.transform.position);
        //if player within attack Range, attack it
        if (enemy.IsPlayerWithinAttackRange())
        {
            enemy.animator.SetBool("Chase", false);
            enemy.TransitionToState(new AttackState());
        }
    }
}
