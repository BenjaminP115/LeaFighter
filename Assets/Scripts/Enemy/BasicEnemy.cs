using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] private new string name;
    [SerializeField] private EnemyData enemyData;

    private bool seesPlayer;
    private bool canMove = true;
    private bool isAttacking;

    private Transform player;

    private NavMeshAgent agent;
    private Animator animator;
    private AnimationState animationState;
    private SpriteRenderer spriteRenderer;
    private Spawner parent;

    void Start()
    {
        parent = transform.parent.GetComponent<Spawner>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = enemyData.MovementSpeed;

        animationState = new AnimationState(animator);

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private int counter = 0;
    private float sum = 0;

    void Update()
    {
        counter++;
        sum += agent.velocity.magnitude;

        if (counter == 10)
        {
            if (!isAttacking && sum / 10 > 0.01f)
                animationState.ChangeState(name + "_Walk");
            else if (!isAttacking)
                animationState.ChangeState(name + "_Idle");

            sum = 0;
            counter = 0;
        }

        if ((player.position - transform.position).magnitude <= enemyData.DetectionDistance && (player.position - parent.transform.position).magnitude <= parent.maxFollowDistance)
        {
            seesPlayer = true;
            agent.speed = enemyData.FollowSpeed;
            agent.SetDestination(player.position);

            if (!isAttacking && (player.position - transform.position).magnitude <= 1.4f)
            {
                isAttacking = true;
                animationState.ChangeState(name + "_Attack");
                Invoke("StopAttack", 0.4f);
            }

            if (player.position.x - transform.position.x < 0) spriteRenderer.flipX = true;
            else if (player.position.x - transform.position.x > 0) spriteRenderer.flipX = false;
        }
        else
        {
            seesPlayer = false;
            agent.speed = enemyData.MovementSpeed;
        }

        if (!isAttacking && canMove && !seesPlayer && !agent.hasPath)
        {
            //animationState.ChangeState(name + "_Idle");
            canMove = false;
            StartCoroutine("Move");
        }
    }

    private void StopAttack()
    {
        isAttacking = false;
        animationState.ChangeState(name + "_Idle");
    }

    private IEnumerator Move()
    {
        Vector2 agentPos = new Vector2(transform.position.x, transform.position.y);
        Vector3 randPos = agentPos + Random.insideUnitCircle * enemyData.MoveDistance;

        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(randPos, path) && path.status == NavMeshPathStatus.PathComplete && (randPos - parent.transform.position).magnitude <= parent.maxWalkDistance)
        {
            if (randPos.x - agentPos.x < 0) spriteRenderer.flipX = true;
            else if (randPos.x - agentPos.x > 0) spriteRenderer.flipX = false;

            yield return new WaitForSeconds(Random.Range(enemyData.WaitTime.x, enemyData.WaitTime.y));
            agent.SetDestination(randPos);
        }

        canMove = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyData.MoveDistance);
    }
}