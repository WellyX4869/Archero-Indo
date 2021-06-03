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
        // Damage Player
        if (enemy.isRanged)
        {
            enemy.ShootProjectile();
        }

        if (!enemy.isRanged && enemy.IsPlayerWithinAttackRange())
        {
            enemy.transform.LookAt(enemy.player.transform.position);
            enemy.player.GetComponent<PlayerHpBar>().GetAttacked(enemy.damage);
        }
    }

    public override void Update(EnemyController enemy)
    {
        enemy.animator.SetBool("Hit", false);
        enemy.TransitionToState(new AttackState());
    }
}
