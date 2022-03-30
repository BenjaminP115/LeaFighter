using UnityEngine;

public class DefaultEnemy : BasicEnemy
{
    protected bool canFollow = true;

    protected void Start()
    {
        base.Start();
    }

    protected void Update()
    {
        if (isDead) return;

        base.Update();

        if (canFollow && (player.position - transform.position).magnitude <= enemyData.DetectionDistance && (player.position - parent.transform.position).magnitude <= parent.maxFollowDistance && !PlayerController.isDead)
        {
            roamBlock = true;
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
            roamBlock = false;
            agent.speed = enemyData.MovementSpeed;
        }
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

        audioManager.Play(Random.Range(2, 4));

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
