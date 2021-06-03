using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public override void EnterState(EnemyController enemy)
    {
        enemy.animator.SetBool("Idle", true);
    }

    public override void Update(EnemyController enemy)
    {
        if(enemy.player == null)
        {
            return;
        }
        else
        {
            enemy.animator.SetBool("Idle", false);
            enemy.TransitionToState(new ChaseState());
        }
    }
}
