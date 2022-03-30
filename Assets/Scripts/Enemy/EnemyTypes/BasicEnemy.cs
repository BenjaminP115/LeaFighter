using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] protected new string name;
    [SerializeField] protected EnemyData enemyData;

    protected bool isDead;
    protected bool canRoam = true;
    protected bool roamBlock;

    protected bool animationBlock;
    protected Vector3 facingDirection;

    protected float health;

    private Animator animator;

    protected Transform player;

    protected NavMeshAgent agent;
    protected AnimationState animationState;
    protected SpriteRenderer spriteRenderer;
    protected Spawner parent;
    protected AudioManager audioManager;

    protected void Start()
    {
        parent = transform.parent.GetComponent<Spawner>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioManager = GetComponent<AudioManager>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = enemyData.MovementSpeed;

        animationState = new AnimationState(animator);

        health = enemyData.Health;

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected int counter = 0;
    protected float sum = 0;
    protected double time = 0;

    protected void Update()
    { 
        if (isDead) return;

        facingDirection = Vector3.right;
        if (spriteRenderer.flipX)
            facingDirection = Vector3.left;

        counter++;
        sum += agent.velocity.magnitude;
        time += Time.deltaTime;

        if (counter == 10)
        {
            if (!animationBlock && sum / 10 > 0.01f)
            {
                if (time >= 0.28f && sum / 10 >= 0.1f)
                {
                    audioManager.Play(Random.Range(0, 2));
                    time = 0;
                }
                animationState.ChangeState(name + "_Walk");
            }
            else if (!animationBlock)
                animationState.ChangeState(name + "_Idle");

            sum = 0;
            counter = 0;
        }

        if (!roamBlock && canRoam && !agent.hasPath)
        {
            canRoam = false;
            StartCoroutine("Move");
        }
    }

    public void Damage(float damageAmount)
    {
        if (isDead) return;

        health -= damageAmount;

        animationState.ChangeState(name + "_Hit");

        if (health <= 0)
        {
            health = 0;
            animationState.ChangeState(name + "_Death");
            isDead = true;
            agent.enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            Spawner.enemyAmount--;
            GameManager.Instance.kills++;
            GameManager.Instance.xp += enemyData.Xp;
        }
    }

    protected IEnumerator Move()
    {
        Vector2 agentPos = new Vector2(transform.position.x, transform.position.y);
        Vector3 randPos = agentPos + Random.insideUnitCircle * enemyData.MoveDistance;

        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(randPos, path) && path.status == NavMeshPathStatus.PathComplete && (randPos - parent.transform.position).magnitude <= parent.maxWalkDistance)
        {
            if (randPos.x - agentPos.x < 0) spriteRenderer.flipX = true;
            else if (randPos.x - agentPos.x > 0) spriteRenderer.flipX = false;

            yield return new WaitForSeconds(Random.Range(enemyData.WaitTime.x, enemyData.WaitTime.y));

            if (!isDead)
                agent.SetDestination(randPos);
        }

        canRoam = true;
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyData.MoveDistance);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + facingDirection * 0.25f, new Vector3(0.75f, 1.25f, 0f));
    }
}