using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyType;
    [SerializeField] private Vector2Int[] enemySpawnAmount;
    [SerializeField] private float maxSpawnDistance = 3f;

    public float maxWalkDistance = 5f;
    public float maxFollowDistance = 7f;

    void Start()
    {
        int[] spawnAmount = new int[enemySpawnAmount.Length];
        int spawnAmountMax = 0;

        for (int i = 0; i < spawnAmount.Length; i++)
        {
            int amount = Random.Range(enemySpawnAmount[i].x, enemySpawnAmount[i].y + 1);
            spawnAmount[i] = amount;
            spawnAmountMax += amount;
        }

        int test = 0;
        int index = 0;
        while (spawnAmountMax != 0)
        {
            Vector2 agentPos = new Vector2(transform.position.x, transform.position.y);
            Vector3 randPos = agentPos + Random.insideUnitCircle * maxSpawnDistance;

            if (NavMesh.SamplePosition(randPos, out NavMeshHit hit, maxSpawnDistance, -NavMesh.GetAreaFromName(LayerMask.LayerToName(gameObject.layer))))
            {
                GameObject spawn = Instantiate(enemyType[index], hit.position, Quaternion.Euler(0, 0, 0), transform);
                spawn.layer = gameObject.layer;
                spawn.GetComponent<NavMeshAgent>().areaMask = 1 << NavMesh.GetAreaFromName(LayerMask.LayerToName(gameObject.layer));
                spawn.GetComponent<NavMeshAgent>().enabled = false;
                spawn.GetComponent<NavMeshAgent>().enabled = true;

                spawnAmountMax--;
                spawnAmount[index]--;

                if (spawnAmount[index] == 0)
                    index++;
            }

            test++;

            if (test >= 100)
                break;
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
