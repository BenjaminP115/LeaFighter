using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;

    private Tilemap[] tilemaps;
    private Vector3 input = Vector3.zero;
    private PlayerController playerController;

    public static SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
        tilemaps = FindObjectsOfType<Tilemap>();
    }

    private void Update()
    {
        if (PlayerController.isDead) return;

        input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f).normalized;

        if (Input.GetKeyDown(KeyCode.A))
        {
            spriteRenderer.flipX = true;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            spriteRenderer.flipX = false;
        }

        if (input.magnitude > 0.1f && !playerController.isAttacking)
            PlayerController.ChangeAnimationState(PlayerAnimationState.Walking);
        else if (!playerController.isAttacking)
            PlayerController.ChangeAnimationState(PlayerAnimationState.Idle);

        foreach (Tilemap tilemap in tilemaps)
        {
            if (tilemap.gameObject.layer > gameObject.layer && tilemap.GetTile(tilemap.WorldToCell(transform.position)))
            {
                Color color = new Color(tilemap.color.r, tilemap.color.b, tilemap.color.g, 0.75f);
                tilemap.color = color;
            }
            else
            {
                Color color = new Color(tilemap.color.r, tilemap.color.b, tilemap.color.g, 1f);
                tilemap.color = color;
            }
        }
    }

    private double time = 0;
    private void FixedUpdate()
    {
        time += Time.deltaTime;
        if (!playerController.isAttacking && !PlayerController.isDead)
        {
            if (time >= 0.28f && input.magnitude >= 0.1f)
            {
                playerController.audioManager.Play(Random.Range(0, 2));
                time = 0;
            }

            transform.Translate(input * movementSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Finish")
        {
            SceneManager.LoadScene("LevelEnd");
        }
    }
}