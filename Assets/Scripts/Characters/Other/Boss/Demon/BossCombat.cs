using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class BossCombat : MonoBehaviour
{
    public Rigidbody2D rb;
   // public Transform target;
    public float timeToTarget;
    public BossStats stats;

    public Transform attackPoint01;
    public float attackRange01 = 0.5f;

    public Transform attackPoint02;
    public float attackRange02 = 0.5f;
    public LayerMask playerLayer;

    public GameObject shield;

    public GameObject chargeAttack;

    int count = 0;

    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }
    public void Walk(Vector2 target)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        rb.velocity = new Vector2(dir.x * stats.walkSpeed, rb.velocity.y);
        stats.Flip(dir.x);
    }
    public void Run(Vector2 target)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        rb.velocity = new Vector2(dir.x * stats.runSpeed, rb.velocity.y);
        stats.Flip(dir.x);
    }

    public void Jump(Vector2 targetPos)
    {
        AudioManager.Instance?.PlaySFX(stats.audioSource, stats.jumpSound);
        Vector2 start = rb.position+stats.offset;
        Vector2 end = targetPos;

        float t = timeToTarget;
        float g = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);

        float vx = (end.x - start.x) / t;
        float vy = (end.y - start.y + 0.5f * g * t * t) / t;

        rb.velocity = new Vector2(vx, vy);
        CameraTrigger.Instance.ShakeCamera(1.5f, 0.2f);
    }

    
    public void DoAttack(int type=1)
    {
        AudioManager.Instance?.PlaySFX(stats.audioSource, stats.attackSound);
        Transform currentAttackPoint = type==1? attackPoint01 : attackPoint02;
        float currentAttackRange = type == 1? attackRange01 : attackRange02;

        Collider2D hit = Physics2D.OverlapCircle(currentAttackPoint.position, currentAttackRange, playerLayer);
        if (hit != null)
        {
            count++; 
            Vector2 hitPosition = hit.ClosestPoint(currentAttackPoint.position);
            PlayerController.Instance.TakeDamage(stats.damage, hitPosition);
        }
    }
    public void Block()
    {
        shield.SetActive(true);
    }
    public void StopBlock()
    {
        shield.SetActive(false);
    }

    public void ChargeAttack()
    {
        chargeAttack.SetActive(true);
    }
    public void StopChargeAttack()
    {
        chargeAttack.SetActive(false);
    }
    void OnDrawGizmosSelected()
    {
         Gizmos.DrawWireSphere(rb.position+stats.offset, 0.1f);
        if (attackPoint01 != null)
            Gizmos.DrawWireSphere(attackPoint01.position, attackRange01);
        if (attackPoint02 != null)
            Gizmos.DrawWireSphere(attackPoint02.position, attackRange02);
    }
}
