using System.ComponentModel;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;

    private bool isAttacking;
    private Vector3 input = Vector3.zero;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AnimationState animationState;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = rb.GetComponent<SpriteRenderer>();
        animator = rb.GetComponent<Animator>();
        animationState = new AnimationState(animator);
    }

    private void Update()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f).normalized;

        if (Input.GetKeyDown(KeyCode.A))
        {
            spriteRenderer.flipX = true;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            spriteRenderer.flipX = false;
        }

        if (!isAttacking && Input.GetButtonDown("Fire1"))
        {
            isAttacking = true;
            ChangeAnimationState(PlayerAnimationState.Attacking);
            Invoke("StopAttack", 0.4f);
        }

        if (input.magnitude > 0.1f && !isAttacking)
            ChangeAnimationState(PlayerAnimationState.Walking);
        else if (!isAttacking)
            ChangeAnimationState(PlayerAnimationState.Idle);
    }

    private void FixedUpdate()
    {
        if (!isAttacking)
            transform.Translate(input * movementSpeed * Time.deltaTime);
    }

    private void StopAttack()
    {
        isAttacking = false;
        ChangeAnimationState(PlayerAnimationState.Idle);
    }

    private void ChangeAnimationState(PlayerAnimationState newState)
    {
        animationState.ChangeState(newState);
    }
}

enum PlayerAnimationState
{
    [Description("Player_Idle")]
    Idle,
    [Description("Player_Walk")]
    Walking,
    [Description("Player_Attack")]
    Attacking,
    [Description("Player_Death")]
    Dead
}