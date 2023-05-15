using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum MF_AnimationState
{
    Idle,
    Walk,
    Run,
    Jump,
    Land,
}
public class MF_AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private MF_AnimationState animationState;

    [Header("Animation Parameter")]
    [Range(0, 1)] [SerializeField] private float grounded;
    [Range(0, 2)] [SerializeField] private float movement;
    [Range(0, 2)] [SerializeField] private float xMove;
    [SerializeField] private float maxStartRun;

    [Header("Animation Name Parameter")]
    private const string groundedName = "Grounded";
    private const string movementName = "Movement";
    private const string xMoveName = "XMove";

    private void Update()
    {
        UpdateAnimationState();
        animator.SetFloat(groundedName, grounded);
        animator.SetFloat(movementName, movement);
        animator.SetFloat(xMoveName, xMove);
    }

    private void UpdateAnimationState()
    {
        switch(animationState)
        {
            case MF_AnimationState.Idle: IdleState(); break;
            case MF_AnimationState.Walk: WalkState(); break;
            case MF_AnimationState.Run: RunState(); break;
            case MF_AnimationState.Jump: JumpState(); break;
        }
    }

    private void IdleState()
    {
        movement = 0;
        xMove = 0;
    }
    private void WalkState()
    {
        movement = 1;
        if(xMove < 1) xMove += Time.deltaTime * 1;
    }
    private void RunState()
    {
        movement = 2;
        if(xMove <maxStartRun) xMove += Time.deltaTime * 1;
        else xMove = 2;
    }

    private void JumpState()
    {
        JumpAround();
    }
    
    [Header("Jump Parameter")]
    [SerializeField] private float jumpForce;
    [SerializeField] bool groundedCheck;
    [SerializeField] float groundDistance; 
    [SerializeField] private LayerMask groundMask;
    [SerializeField]private Rigidbody2D rb;

    private void JumpAround()
    {
        groundedCheck = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundMask);
        if (!groundedCheck) return;
        rb.velocity = Vector2.up * jumpForce;
    }
}
