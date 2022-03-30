using System.ComponentModel;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject lvlUp;

    private GameManager gm;

    public static bool isDead;
    [HideInInspector] public bool isAttacking;

    private int currLevel;
    private Vector3 facingDirection;
    private bool blockAnimations;
    private static PlayerController instance;
    private static AnimationState animationState;
    [HideInInspector] public AudioManager audioManager;

    private Animator animator;

    private void Start()
    {
        gm = GameManager.Instance;
        instance = this;
        animator = GetComponent<Animator>();
        audioManager = GetComponent<AudioManager>();
        animationState = new AnimationState(animator);
    }

    void Update()
    {
        facingDirection = Vector3.right;
        if (PlayerMovement.spriteRenderer.flipX)
            facingDirection = Vector3.left;

        if (!isAttacking && !isDead && Input.GetButtonDown("Fire1"))
            Attack();

        if (gm.level > currLevel)
        {
            currLevel = gm.level;

            lvlUp.SetActive(true);

            Invoke("DeaktivateLvlUp", 1.24f);

            gm.damageLevel++;
            gm.healthLevel++;
            gm.defenseLevel++;
            gm.attrbPoints++;

            gm.health = gm.maxHealth;
        }
    }

    private void DeaktivateLvlUp()
    {
        lvlUp.SetActive(false);
    }

    private void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + facingDirection * 0.25f, new Vector3(0.75f, 1.25f, 0f), 0f);

        audioManager.Play(Random.Range(2, 4));

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Enemy")
            {
                collider.gameObject.GetComponent<BasicEnemy>().Damage(gm.damage);
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
        gm.health -= damageAmount / gm.defense;

        if (gm.health <= 0)
        {
            gm.health = 0;
            ChangeAnimationState(PlayerAnimationState.Death, 0.4f);
            isDead = true;
        }
        else
            ChangeAnimationState(PlayerAnimationState.Hit, 0.4f);
    }

    public static void ChangeAnimationState(PlayerAnimationState playerAnimationState, float time = 0)
    {
        if (isDead) return;

        if (!instance.blockAnimations)
            animationState.ChangeState(playerAnimationState);


        switch (playerAnimationState)
        {
            case PlayerAnimationState.Hit:
            case PlayerAnimationState.Attacking:
                instance.blockAnimations = true;
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