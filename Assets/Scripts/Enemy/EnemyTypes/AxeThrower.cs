using UnityEngine;

public class AxeThrower : DefaultEnemy
{
    [SerializeField] private float throwDistance;
    [SerializeField] private float throwDelay;

    [SerializeField] private GameObject axe;

    void Start()
    {
        base.Start();
    }

    private double time;
    void Update()
    {
        if (isDead) return;

        base.Update();

        if (!animationBlock)
            time += Time.deltaTime;

        if (!PlayerController.isDead && time >= throwDelay && player.gameObject.layer == gameObject.layer && (player.position - transform.position).magnitude <= throwDistance)
        {
            animationState.ChangeState(name + "_Special1");
            roamBlock = true;
            canFollow = false;
            agent.SetDestination(transform.position);
            animationBlock = true;
            Invoke("ThrowAxe", 0.2f);
            Invoke("StopAxeThrow", 1.1f);
            time = 0;

            if (player.position.x - transform.position.x < 0) spriteRenderer.flipX = true;
            else if (player.position.x - transform.position.x > 0) spriteRenderer.flipX = false;
        }
    }

    private void ThrowAxe()
    {
        GameObject spawn = Instantiate(axe, transform.position, Quaternion.identity);
        spawn.GetComponent<Axe>().moveDir = (player.position - transform.position).normalized * 3;
    }

    private void StopAxeThrow()
    {
        canFollow = true;
        roamBlock = false;
        animationBlock = false;
    }

    void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, throwDistance);
    }
}
