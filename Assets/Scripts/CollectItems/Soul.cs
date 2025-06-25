using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    public float attractSpeed = 8f;
    private bool isAttracted = false;
    private Transform player;

    void Update()
    {
        if (isAttracted && player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, attractSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.position) < 0.2f)
            {
 
                CollectSoul();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isAttracted == false)
        {
            isAttracted = true;
            player = collision.transform;
        }
    }

    void CollectSoul()
    {
        GameController.Instance?.CollectSouls(1);
        AudioManager.Instance?.PlaySFX(PlayerController.Instance.audioSource, PlayerController.Instance.collectSound,0.3f);
        Destroy(gameObject);
    }
}
