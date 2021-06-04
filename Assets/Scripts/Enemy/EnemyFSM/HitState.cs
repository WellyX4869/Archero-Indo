using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : State
{
    float elapsedTime = 0f;

    public override void EnterState(EnemyController enemy)
    {
        enemy.animator.SetBool("Hit", true);
     
        if (!enemy.isRanged)
        {
            enemy.transform.LookAt(enemy.player.transform.position);      
        }
    }

    public override void Update(EnemyController enemy)
    {
        enemy.animator.SetBool("Hit", false);
        enemy.TransitionToState(new AttackState());
    }
}
