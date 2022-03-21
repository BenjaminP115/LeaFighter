using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;

    private AnimationState currentState = AnimationState.Idle;
    private bool isAttacking = false;
    private Vector3 input = Vector3.zero;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = rb.GetComponent<SpriteRenderer>();
        animator = rb.GetComponent<Animator>();
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
            ChangeAnimationState(AnimationState.Attacking);
            Invoke("StopAttack", 0.4f);
        }

        if (input.magnitude > 0.1f && !isAttacking)
            ChangeAnimationState(AnimationState.Walking);
        else if (!isAttacking)
            ChangeAnimationState(AnimationState.Idle);
    }

    private void FixedUpdate()
    {
        if (!isAttacking)
            transform.Translate(input * movementSpeed * Time.deltaTime);
    }

    private void StopAttack()
    {
        isAttacking = false;
        ChangeAnimationState(AnimationState.Idle);
    }

    private void ChangeAnimationState(AnimationState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        string state = "";

        switch (newState)
        {
            case AnimationState.Walking : state = "Player_Walk"; break;
            case AnimationState.Attacking: state = "Player_Attack"; break;
            default: state = "Player_Idle"; break;
        }

        animator.Play(state);
    }
}

enum AnimationState
{
    Idle,
    Walking,
    Attacking
}