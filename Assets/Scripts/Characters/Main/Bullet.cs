using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 35f;
    public Rigidbody2D rb;
    public bool isFacingRight = true;
    public ItemData itemData;
    public AudioClip bulletHitSound;
    public AudioSource audioSource;
    private float damage;
    private void Awake()
    {
        //itemData.UseItem();
        ItemDataManager.Instance.DecreaseItem(itemData, 1);
        damage = PlayerController.Instance.GetAttackDamage();
        StatusBar statusBar = FindObjectOfType<StatusBar>();
        if (statusBar != null)
        {
            statusBar.UpdateBulletText();
        }
        Destroy(this, 3f);
    }

    private void Update()
    {
        Vector2 position = transform.position;
        Vector2 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector2 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));


        if (position.x < bottomLeft.x || position.x > topRight.x)
        {
            Destroy(gameObject);
        }
    }


    public void SetUp(bool isRight, Vector3 position)
    {
        isFacingRight=isRight;
        transform.position = position;
        Vector3 bulletScale = transform.localScale;
        bulletScale.y *= isFacingRight ? 1 : -1;
        transform.localScale = bulletScale;
        rb.velocity = new Vector2(isFacingRight ? speed:- speed, rb.velocity.y);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Vector2 hitPosition = collision.ClosestPoint(transform.position);
            AudioManager.Instance?.PlaySFX(audioSource, bulletHitSound);
            EnemyHeath enemyHealth = collision.gameObject.GetComponent<EnemyHeath>();
            BossHeath bossHeath = collision.gameObject.GetComponent<BossHeath>(); 
            DragonHeath dragonHeath = collision.gameObject.GetComponent<DragonHeath>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage, hitPosition);
            }
            else if (bossHeath != null)
            {
                bossHeath.TakeDamage(damage, hitPosition);
            }
            else if(dragonHeath!= null)
            {
                dragonHeath.TakeDamage(damage, hitPosition);
            }
            

            Destroy(gameObject);
        }
        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

}
