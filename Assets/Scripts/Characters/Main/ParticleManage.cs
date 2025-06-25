using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ParticleManage : MonoBehaviour
{
    [SerializeField] ParticleSystem movementParticle;

    [Range(0, 10)]
    [SerializeField] int occurAfterVelcity;//ngưỡng vận tốc tối thiểu để phát hạt

    [Range(0, 0.2f)]
    [SerializeField] float dustFormationPeriod; // thời gian giữa 2 lần phát hạt.
    [SerializeField] Rigidbody2D playerRb;
    float counter;
    bool isGrounded;


    [SerializeField] ParticleSystem fallParticle;
    [SerializeField] ParticleSystem jumpParticle;

    private PlayerController controller;
    private void Start()
    {
        controller = PlayerController.Instance;
    }
    private void Update()
    {
        counter += Time.deltaTime;
        if (isGrounded && Mathf.Abs(playerRb.velocity.x) > occurAfterVelcity)
        {
            if (counter > dustFormationPeriod)
            {
                movementParticle.Play();
                counter = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {   
            if(!fallParticle.isPlaying)
                fallParticle.Play();
            isGrounded = true;
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(controller.audioSource,controller.landSound);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            jumpParticle.Play();
            AudioManager.Instance?.PlaySFX(controller.audioSource, controller.jumpSound);
        }
    }

}
