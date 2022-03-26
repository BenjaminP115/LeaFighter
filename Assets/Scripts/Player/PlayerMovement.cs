using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private Tilemap[] tilemaps;

    private bool isAttacking;
    private Vector3 input = Vector3.zero;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tilemaps = FindObjectsOfType<Tilemap>();

        foreach (Tilemap tilemap in tilemaps)
        {
            Debug.Log(tilemap.gameObject.layer);
            Debug.Log(LayerMask.LayerToName(tilemap.gameObject.layer));
        }
    }

    private void Update()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f).normalized;

        if (Input.GetKeyDown(KeyCode.A) && !PlayerController.isDead)
        {
            spriteRenderer.flipX = true;
        }
        else if (Input.GetKeyDown(KeyCode.D) && !PlayerController.isDead)
        {
            spriteRenderer.flipX = false;
        }

        if (!isAttacking && !PlayerController.isDead && Input.GetButtonDown("Fire1"))
        {
            isAttacking = true;
            PlayerController.ChangeAnimationState(PlayerAnimationState.Attacking, 0.4f);
            Invoke("StopAttack", 0.4f);
        }

        if (input.magnitude > 0.1f && !isAttacking)
            PlayerController.ChangeAnimationState(PlayerAnimationState.Walking);
        else if (!isAttacking)
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

    private void FixedUpdate()
    {
        if (!isAttacking && !PlayerController.isDead)
            transform.Translate(input * movementSpeed * Time.deltaTime);
    }

    private void StopAttack()
    {
        isAttacking = false;
        PlayerController.ChangeAnimationState(PlayerAnimationState.Idle);
    }
}