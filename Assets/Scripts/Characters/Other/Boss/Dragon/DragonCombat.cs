using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonCombat : MonoBehaviour
{
    public Rigidbody2D rb;
    // public Transform target;
    public float timeToTarget;
    public DragonStats stats;
    [SerializeField] private DragonAnimator anim;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    public void Appear()
    {
        anim.Appear();
        stats.audioSource.PlayOneShot(stats.awakeSound);
        Instantiate(stats.stoneBrokenPrefab,stats.groundCheck.transform.position,Quaternion.identity);
        CameraTrigger.Instance.ShakeCamera(stats.moveShakeIntensity, stats.moveShakeDuration);
        Invoke("Idle", anim.appearAnimation.length);
    }
    public void Idle()
    {
        
        anim.Idle();
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
        Vector2 start = rb.position + stats.offset;
        Vector2 end = targetPos;

        float t = timeToTarget;
        float g = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);

        float vx = (end.x - start.x) / t;
        float vy = (end.y - start.y + 0.5f * g * t * t) / t;


        rb.velocity = new Vector2(vx, vy);
        CameraTrigger.Instance.ShakeCamera(1.5f, 0.2f);

    }


    public void DoAttack(int type = 1)
    {
        Transform currentAttackPoint = type == 1 ? stats.attackPoint01 : stats.attackPoint02;
        float currentAttackRange = type == 1 ? stats.attackRange01 : stats.attackRange02;

        Collider2D hit = Physics2D.OverlapCircle(currentAttackPoint.position, currentAttackRange, stats.playerLayer);
        if (hit != null)
        {
            Vector2 hitPosition = hit.ClosestPoint(currentAttackPoint.position);
            PlayerController.Instance.TakeDamage(stats.damage, hitPosition);
        }
        if (type == 1)
        {
            Instantiate(stats.stoneBrokenPrefab, stats.attackPoint01.position, Quaternion.identity);
        }
        else
        {
            Instantiate(stats.stoneBrokenPrefab, stats.attackPoint02.position, Quaternion.identity);
        }

    }

    public void FireAttack()
    {
        AudioManager.Instance.PlaySFX(stats.audioSource, stats.fireAttackSound);
        anim.Attack(3);
    }

    public void Shout()
    {
        stats.IsAttacking = false;
        anim.Shout();
    //    anim.StopAttack();
       // anim.TurnOnController();
        
    }

}
