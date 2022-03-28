using UnityEngine;

public class AxeThrower : DefaultEnemy
{
    [SerializeField] private float throwDistance;
    [SerializeField] private GameObject axe;

    void Start()
    {
        base.Start();
    }

    private double time;
    void Update()
    {
        time += Time.deltaTime;
        base.Update();

        if (time >= 1 && player.gameObject.layer == gameObject.layer && (player.position - transform.position).magnitude <= throwDistance)
        {
            GameObject spawn = Instantiate(axe, transform.position, Quaternion.identity);
            spawn.GetComponent<Axe>().facingDirection = player.position - transform.position;
            time = 0;
        }
    }

    void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
    }
}
