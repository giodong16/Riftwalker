using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    public int damage = 500;
    public GameObject hitParticle;
    public bool isShowEffectWithGround = false;

    public float lifeTime = 5f;
    private AudioSource audioSource;
    private void Start()
    {
        Destroy(gameObject,lifeTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 hitPos = collision.ClosestPoint(transform.position);
           
            PlayerController.Instance.TakeDamage(damage,hitPos);
            if (hitParticle != null) {
                GameObject explo =  Instantiate(hitParticle, transform.position, Quaternion.identity);
                audioSource = explo.GetComponent<AudioSource>();
                AudioManager.Instance?.PlaySFX(audioSource, audioSource.clip);

            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground"))
        {
            if (hitParticle != null && isShowEffectWithGround)
            {
                GameObject explo = Instantiate(hitParticle, transform.position, Quaternion.identity);
                audioSource = explo.GetComponent<AudioSource>();
                AudioManager.Instance?.PlaySFX(audioSource, audioSource.clip);
            }
            Destroy(gameObject);
        }
    }
}
