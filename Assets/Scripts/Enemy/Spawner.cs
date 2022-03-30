using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public struct Enemy
    {
        public GameObject enemyType;
        public Vector2Int enemySpawnAmount;
    }

    [SerializeField] private Enemy[] enemies;
    [SerializeField] private float maxSpawnDistance = 3f;

    public static int enemyAmount;

    public float maxWalkDistance = 5f;
    public float maxFollowDistance = 7f;

    void Start()
    {
        int[] spawnAmount = new int[enemies.Length];
        int spawnAmountMax = 0;

        for (int i = 0; i < spawnAmount.Length; i++)
        {
            int amount = Random.Range(enemies[i].enemySpawnAmount.x, enemies[i].enemySpawnAmount.y + 1);
            spawnAmount[i] = amount;
            spawnAmountMax += amount;
        }

        int test = 0;
        int index = 0;
        while (spawnAmountMax != 0)
        {
            Vector2 agentPos = new Vector2(transform.position.x, transform.position.y);
            Vector3 randPos = agentPos + Random.insideUnitCircle * maxSpawnDistance;

            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(transform.position, randPos, -NavMesh.GetAreaFromName(LayerMask.LayerToName(gameObject.layer)), path) && path.status == NavMeshPathStatus.PathComplete)
            {
                enemyAmount++;

                GameObject spawn = Instantiate(enemies[index].enemyType, randPos, Quaternion.identity, transform);
                spawn.layer = gameObject.layer;
                //spawn.GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID(LayerMask.LayerToName(gameObject.layer));
                SpriteRenderer[] spriteRenderers = spawn.GetComponentsInChildren<SpriteRenderer>();

                foreach (SpriteRenderer spriteRenderer in spriteRenderers)
                {
                    spriteRenderer.sortingLayerID = SortingLayer.NameToID(LayerMask.LayerToName(gameObject.layer));
                }

                NavMeshAgent spawnAgent = spawn.GetComponent<NavMeshAgent>();
                spawnAgent.areaMask = 1 << NavMesh.GetAreaFromName(LayerMask.LayerToName(gameObject.layer));
                spawnAgent.enabled = false;
                spawnAgent.enabled = true;

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
