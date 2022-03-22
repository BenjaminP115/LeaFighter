using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] private new string name;
    [SerializeField] private EnemyData enemyData;

    private Transform player;
    private bool seesPlayer = false;
    private bool canMove = true;
    private Vector3 spawnPos;
    private NavMeshAgent agent;
    private Animator animator;
    private AnimationState animationState;
    private SpriteRenderer spriteRenderer;
    private Spawner parent;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = enemyData.MovementSpeed;

        animationState = new AnimationState(animator);

        spawnPos = transform.parent.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Awake()
    {
        parent = transform.parent.GetComponent<Spawner>();
    }

    void Update()
    {
        if ((player.position - transform.position).magnitude <= enemyData.DetectionDistance && (player.position - spawnPos).magnitude <= parent.maxFollowDistance)
        {
            seesPlayer = true;
            agent.speed = enemyData.FollowSpeed;
            agent.SetDestination(player.position);
            animationState.ChangeState(name + "_Walk");

            if (player.position.x - transform.position.x < 0) spriteRenderer.flipX = true;
            else if (player.position.x - transform.position.x > 0) spriteRenderer.flipX = false;
        }
        else
        {
            seesPlayer = false;
            agent.speed = enemyData.MovementSpeed;
        }

        if (canMove && !seesPlayer && !agent.hasPath)
        {
            animationState.ChangeState(name + "_Idle");
            canMove = false;
            StartCoroutine("Move");
        }
    }

    private IEnumerator Move()
    {
        Vector2 agentPos = new Vector2(transform.position.x, transform.position.y);
        Vector3 randPos = agentPos + Random.insideUnitCircle * enemyData.MoveDistance;

        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(randPos, path) && path.status == NavMeshPathStatus.PathComplete && (randPos - spawnPos).magnitude <= parent.maxWalkDistance)
        {
            if (randPos.x - agentPos.x < 0) spriteRenderer.flipX = true;
            else if (randPos.x - agentPos.x > 0) spriteRenderer.flipX = false;

            yield return new WaitForSeconds(Random.Range(enemyData.WaitTime.x, enemyData.WaitTime.y));
            agent.SetDestination(randPos);
            animationState.ChangeState(name + "_Walk");
        }

        canMove = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyData.MoveDistance);
    }
}