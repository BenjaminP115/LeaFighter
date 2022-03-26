using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static float damage;
    private static float health = 2000;
    private static float defense;

    public static bool isDead;

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

    }

    public static void Damage(float damageAmount)
    {
        health -= damageAmount;

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
            case PlayerAnimationState.Attacking:
            case PlayerAnimationState.Hit:
                blockAnimations = true;
                instance.Invoke("UnblockAnimations", time);
                break;
        }
    }

    private void UnblockAnimations()
    {
        Debug.Log("test");
        blockAnimations = false;
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