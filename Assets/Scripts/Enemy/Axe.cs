using UnityEngine;

public class Axe : MonoBehaviour
{
    [SerializeField] private float damage;
    public Vector3 moveDir = Vector3.right;

    private AudioManager audioManager;

    private double time;
    private double destroyTime;


    private void Start()
    {
        audioManager = GetComponent<AudioManager>();
    }

    void Update()
    {
        destroyTime += Time.deltaTime;
        time += Time.deltaTime;

        if (time >= 0.1f)
        {
            audioManager.Play(Random.Range(0, 1));
            time = 0;
        }

        if (destroyTime >= 5) Destroy(gameObject);

        transform.Translate(moveDir * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.gameObject.GetComponent<PlayerController>().Damage(damage);

        Destroy(gameObject);
    }
}
