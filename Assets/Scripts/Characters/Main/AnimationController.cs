using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public PlayerMovement playerMovement;
    private PlayerController controller;
    [SerializeField] private Rigidbody2D rb;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        controller = PlayerController.Instance;
    }

    private void Update()
    {
        HandleState();
        HandleAnimator();
        
    }

    private void HandleAnimator()
    {
        _animator.SetBool("isGrounded", playerMovement.IsGrounded);
        _animator.SetBool("isRunning", playerMovement.IsRunning);
        _animator.SetFloat("velocityX", rb.velocity.x);
        _animator.SetFloat("velocityY", rb.velocity.y);
        _animator.SetBool("isSliding", playerMovement.IsSliding);
        _animator.SetBool("canController", controller.CanController);
    }
    private void HandleState()
    {
        if (playerMovement.HorizontalMove != 0f)
        {
            playerMovement.IsRunning = true;
        }
        else
        {
            playerMovement.IsRunning = false;
        }

        //jump
        if (controller._isClimbing)
        {
            playerMovement.CanJump = false;
        }
        else
        {
            playerMovement.CanJump = true;
        }

        //dash
        if (controller._isClimbing)
        {
            playerMovement.CanDash = false;
        }
        else
        {
            playerMovement.CanDash = true;
        }

    }
    public void EndAttack()
    {
        controller.IsAttacking = false;
        _animator.SetBool("isAttacking", controller.IsAttacking);
    }
    public void TurnOnController()
    {
        controller.CanController = true;
        controller.IsAttacking = false;

    }

    
}



