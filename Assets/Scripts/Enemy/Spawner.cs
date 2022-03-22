using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Vector2Int enemySpawnAmount;
    [SerializeField] private GameObject enemyType;
    [SerializeField] private float maxSpawnDistance = 3f;

    public float maxWalkDistance = 5f;
    public float maxFollowDistance = 7f;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        int spawnAmount = Random.Range(enemySpawnAmount.x, enemySpawnAmount.y + 1);
        Debug.Log(spawnAmount);

        while (spawnAmount != 0)
        {
            Vector2 agentPos = new Vector2(transform.position.x, transform.position.y);
            Vector3 randPos = agentPos + Random.insideUnitCircle * maxSpawnDistance;

            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(randPos, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                Instantiate(enemyType, randPos, Quaternion.Euler(90, 0, 0), transform);
                spawnAmount--;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxWalkDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxSpawnDistance);
    }
}
