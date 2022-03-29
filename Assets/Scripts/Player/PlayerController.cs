using System.ComponentModel;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float health = 2000;
    [SerializeField] private float defense;

    public static bool isDead;
    public static bool isAttacking;

    private Vector3 facingDirection;
    private static bool blockAnimations;
    private static PlayerController instance;
    private static AnimationState animationState;

    private Animator animator;

    private void Start()
    {
        instance = this;
        animator = GetComponent<Animator>();
        animationState = new AnimationState(animator);
    }

    void Update()
    {
        facingDirection = Vector3.right;
        if (PlayerMovement.spriteRenderer.flipX)
            facingDirection = Vector3.left;

        if (!isAttacking && !isDead && Input.GetButtonDown("Fire1"))
            Attack();
    }

    private void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + facingDirection * 0.25f, new Vector3(0.75f, 1.25f, 0f), 0f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Enemy")
            {
                collider.gameObject.GetComponent<BasicEnemy>().Damage(damage);
            }
        }

        isAttacking = true;
        ChangeAnimationState(PlayerAnimationState.Attacking, 0.4f);
        Invoke("StopAttack", 0.4f);

    }

    private void StopAttack()
    {
        isAttacking = false;
        ChangeAnimationState(PlayerAnimationState.Idle);
    }

    public void Damage(float damageAmount)
    {
        health -= damageAmount;

        ChangeAnimationState(PlayerAnimationState.Hit, 0.4f);

        if (health <= 0)
        {
            health = 0;
            ChangeAnimationState(PlayerAnimationState.Death, 0.4f);
            isDead = true;
        }
    }

    public static void ChangeAnimationState(PlayerAnimationState playerAnimationState, float time = 0)
    {
        if (isDead) return;

        if (!blockAnimations)
            animationState.ChangeState(playerAnimationState);

        switch (playerAnimationState)
        {
            case PlayerAnimationState.Hit:
            case PlayerAnimationState.Attacking:
                blockAnimations = true;
                instance.Invoke("UnblockAnimations", time);
                break;
        }
    }

    private void UnblockAnimations()
    {
        blockAnimations = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + facingDirection * 0.25f, new Vector3(0.75f, 1.25f, 0f));
    }
}

public enum PlayerAnimationState
{
    [Description("Player_Idle")]
    Idle,
    [Description("Player_Walk")]
    Walking,
    [Description("Player_Attack")]
    Attacking,
    [Description("Player_Death")]
    Death,
    [Description("Player_Hit")]
    Hit
}