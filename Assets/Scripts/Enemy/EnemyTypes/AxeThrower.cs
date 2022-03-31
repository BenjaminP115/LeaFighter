using UnityEngine;

public class AxeThrower : DefaultEnemy
{
    [SerializeField] private float throwDistance;
    [SerializeField] private float throwDelay;

    [SerializeField] private GameObject axe;

    protected void Start()
    {
        base.Start();
    }

    private double timeBetwwen;
    protected void Update()
    {
        if (isDead) return;

        base.Update();

        if (!animationBlock)
            timeBetwwen += Time.deltaTime;

        if (!PlayerController.isDead && timeBetwwen >= throwDelay && player.gameObject.layer == gameObject.layer && (player.position - transform.position).magnitude <= throwDistance)
        {
            animationState.ChangeState(name + "_AxeThrow");
            roamBlock = true;
            canFollow = false;
            agent.SetDestination(transform.position);
            animationBlock = true;
            Invoke("ThrowAxe", 0.2f);
            Invoke("StopAxeThrow", 1.1f);
            timeBetwwen = 0;

            if (player.position.x - transform.position.x < 0) spriteRenderer.flipX = true;
            else if (player.position.x - transform.position.x > 0) spriteRenderer.flipX = false;
        }
    }

    private void ThrowAxe()
    {
        audioManager.Play(4);
        GameObject spawn = Instantiate(axe, transform.position, Quaternion.identity);
        spawn.GetComponent<Axe>().moveDir = (player.position - transform.position).normalized * 3;
        spawn.layer = gameObject.layer;
    }

    private void StopAxeThrow()
    {
        canFollow = true;
        roamBlock = false;
        animationBlock = false;
    }

    protected void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, throwDistance);
    }
}
