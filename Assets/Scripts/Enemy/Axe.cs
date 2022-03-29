using UnityEngine;

public class Axe : MonoBehaviour
{
    [SerializeField] private float damage;
    public Vector3 moveDir = Vector3.right;

    private double time;
    void Update()
    {
        time += Time.deltaTime;

        if (time >= 10) Destroy(gameObject);

        transform.Translate(moveDir * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.gameObject.GetComponent<PlayerController>().Damage(damage);

        Destroy(gameObject);
    }
}
