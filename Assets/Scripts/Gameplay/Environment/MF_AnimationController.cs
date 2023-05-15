using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
}
