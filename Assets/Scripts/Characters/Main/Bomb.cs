using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Transform offset;
    public float radius = 1.5f;
    public float force = 700f;
    public float durationTime = 0.2f;
    public float speed = 30f;
    public ItemData item;
    private bool hasExploded = false;

    public GameObject explodeParticle;
    public AudioClip explosionSound;
    private bool isFacingRight;
    private float damage;

    public bool IsFacingRight { get => isFacingRight; set => isFacingRight = value; }
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
       // item.UseItem();
        ItemDataManager.Instance.DecreaseItem(item,1);
        damage = PlayerController.Instance.GetAttackDamage();
    }
    public void SetUp(Vector3 startPos, Vector3 targetPos)
    {
        transform.position = startPos;

        Vector2 direction = (targetPos - startPos).normalized;
        rb.velocity = direction * speed;

        // Xoay bom 
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("ShieldEnemy") || collision.CompareTag("Ground"))
        {
            if (!hasExploded)
            {
                hasExploded = true;
                GameObject explosionParticle =  Instantiate(explodeParticle, transform.position, Quaternion.identity);
                AudioSource audioSource = explosionParticle.GetComponent<AudioSource>();
                if(audioSource != null)
                {
                    AudioManager.Instance?.PlaySFX(audioSource, explosionSound);
                }
                Explode();
            }

        }
    }

    private void Explode()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(offset.position, radius);
        foreach (Collider2D collider in colliders)
        {
            EnemyHeath enemyHeath = collider.GetComponent<EnemyHeath>();
            BossHeath bossHeath = collider.GetComponent<BossHeath>();
            DragonHeath dragonHeath = collider.GetComponent<DragonHeath>();
            PlayerController player = collider.GetComponent<PlayerController>(); 
            
            Vector3 hitPosition = collider.ClosestPoint(transform.position);

            // Xử lý trừ máu quái
            if (enemyHeath != null)
               
                enemyHeath.TakeDamage(damage, hitPosition);

            if (bossHeath != null)
                bossHeath.TakeDamage(damage, hitPosition);

            if (dragonHeath != null)
                dragonHeath.TakeDamage(damage, hitPosition);

            if (player != null)
            {
                player.TakeDamage(damage, hitPosition); 
            }
        }

        CameraTrigger.Instance.ShakeCamera(3, 0.3f);
        Destroy(gameObject);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(offset.position, radius);
    }
}
