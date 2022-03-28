using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemy : BasicEnemy
{

    protected void Start()
    {
        base.Start();
    }

    protected void Update()
    {
        if (isDead) return;

        if ((player.position - transform.position).magnitude <= enemyData.DetectionDistance && (player.position - parent.transform.position).magnitude <= parent.maxFollowDistance && !PlayerController.isDead)
        {
            canMove = false;
            agent.speed = enemyData.FollowSpeed;
            agent.SetDestination(player.position);

            if (!animationBlock && player.gameObject.layer == gameObject.layer && (player.position - transform.position).magnitude <= enemyData.AttackDistance)
            {
                animationBlock = true;
                animationState.ChangeState(name + "_Attack");
                Attack();
            }

            if (player.position.x - transform.position.x < 0) spriteRenderer.flipX = true;
            else if (player.position.x - transform.position.x > 0) spriteRenderer.flipX = false;
        }
        else
        {
            canMove = true;
            agent.speed = enemyData.MovementSpeed;
        }

        base.Update();
    }

    protected void StopAttack()
    {
        animationBlock = false;
        if (!isDead)
            animationState.ChangeState(name + "_Idle");
    }

    protected void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + facingDirection * 0.25f, new Vector3(0.75f, 1.25f, 0f), 0f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Player")
            {
                collider.gameObject.GetComponent<PlayerController>().Damage(enemyData.Damage);
            }
        }

        Invoke("StopAttack", 0.4f);
    }

    protected void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
    }
}
