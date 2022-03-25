using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;

    private bool isAttacking;
    private Vector3 input = Vector3.zero;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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