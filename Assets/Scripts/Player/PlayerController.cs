using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Damage;
    public float Health;
    public float Defense;
    public float Speed;
    public float EnemyDamage;

    private Rigidbody2D rb;
    private Animator animator;
    private AnimationState animationState;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = rb.GetComponent<Animator>();
        animationState = new AnimationState(animator);
    }

    private float DamageGet(float enemydamage, float defense)
    {
        float damagegot;
        EnemyDamage = enemydamage;
        Defense = defense;

        damagegot = enemydamage / defense;

        return damagegot;
    }
    private void Death(float Health)
    {
        if (Health == 0)
        {
            animationState.ChangeState(PlayerAnimationState.Dead);
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Health -= DamageGet(EnemyDamage, Defense);
        }
    }
}
