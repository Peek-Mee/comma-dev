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
}
public class MF_AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private MF_AnimationState animationState;

    [Header("Animation Parameter")]
    [Range(0, 1)] [SerializeField] private float grounded;
    [Range(0, 2)] [SerializeField] private float movement;
    [Range(0, 2)] [SerializeField] private float xMove;
    [SerializeField] private float yMove;
    [SerializeField] private float maxStartRun;
    private bool isLanding;

    [Header("Animation Name Parameter")]
    private const string groundedName = "Grounded";
    private const string movementName = "Movement";
    private const string xMoveName = "XMove";
    private const string yMoveName = "YMove";
    private const string isLandingName = "IsLanding";

    private void Update()
    {
        groundedCheck = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundMask);
        UpdateAnimationState();
        animator.SetFloat(groundedName, grounded);
        animator.SetFloat(movementName, movement);
        animator.SetFloat(xMoveName, xMove);
        animator.SetFloat(yMoveName, yMove);
        animator.SetBool(isLandingName, isLanding);
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
        grounded = 0;
        movement = 0;
        xMove = 0;
        yMove = 0;
    }
    private void WalkState()
    {
        grounded = 0;
        movement = 1;
        yMove = 0;
        if(xMove < 1) xMove += Time.deltaTime * 1;
    }
    private void RunState()
    {
        grounded = 0;
        movement = 2;
        yMove = 0;
        if(xMove <maxStartRun) xMove += Time.deltaTime * 1;
        else xMove = 2;
    }

    private void JumpState()
    {
        grounded = 1;
        JumpAround();
        
    }
    
    [Header("Jump Parameter")]
    [SerializeField] private float jumpForce;
    [SerializeField] private bool isAiring = false;
    [SerializeField] private float waitJump;
    [SerializeField] private float waitLand;
    [SerializeField] bool groundedCheck;
    [SerializeField] float groundDistance; 
    [SerializeField] private LayerMask groundMask;
    [SerializeField]private Rigidbody2D rb;

    private void JumpAround()
    {
        // float min = 0;
        // float max = 1.52f;
        // float duration = 1f;
        // yMove = Mathf.Lerp(min, max, Mathf.PingPong(Time.time / duration, 1));
        if (groundedCheck)
        {
            if (isAiring)
            {
                yMove = 0.01f;
                isLanding = true;
                StartCoroutine(WaitLanding());
            }
            else
            {
                StartCoroutine(WaitJump());
                yMove += Time.deltaTime * 1;
            }
        }
        else
        {
            
        }

        IEnumerator WaitLanding()
        {
            yield return new WaitForSeconds(waitLand);
            isLanding = false;
            isAiring = false;
            
        }
        IEnumerator WaitJump()
        {
            yield return new WaitForSeconds(waitJump);
            rb.velocity = Vector2.up * jumpForce;
            yield return new WaitForSeconds(0.2f);
            isAiring = true;
        }
    }
}
