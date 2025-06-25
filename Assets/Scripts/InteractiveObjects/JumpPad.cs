using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpForce = 5f;
    public Animator anim;
    bool isTurnOn = false;
    Rigidbody2D playerRb;
    AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTurnOn)
        {
            isTurnOn = true;
            playerRb = collision.GetComponent<Rigidbody2D>();
            if(audioSource != null)
            {
                audioSource.PlayOneShot(audioSource.clip);
            }
            AddForce();
            anim.Play("TurnOn");

        }
    }
    public void TurnOff()
    {
        isTurnOn = false;
    }
    public void AddForce()
    {
        if (playerRb != null) {
            playerRb.velocity = new Vector2(playerRb.velocity.x, 0f);
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

}
