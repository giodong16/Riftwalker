using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractive : MonoBehaviour
{
    public AudioClip grassSound;
    public AudioClip moveOnWooden;
    public PlayerMovement playerMovement;
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Vine"))
        {
            AudioManager.Instance?.PlaySFXLoop(grassSound, 1f);
        }
/*        else if (collision.CompareTag("Bridge") && playerMoverment.IsGrounded && playerMoverment.HorizontalMove !=0)
        {
            AudioManager.Instance?.PlaySFXLoop(moveOnWooden, 1f);
        }
        else if(collision.CompareTag("Bridge") && playerMoverment.HorizontalMove == 0)
        {
            AudioManager.Instance?.StopSFXLoop();
        }*/
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Vine"))
        {
            AudioManager.Instance?.StopSFXLoop();
        }
/*        else if (collision.CompareTag("Bridge"))
        {
            AudioManager.Instance?.StopSFXLoop();
        }*/
    }
/*    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }*/
}
