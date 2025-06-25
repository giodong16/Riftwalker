using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragonAI : MonoBehaviour
{
    
    public Transform player;
    [SerializeField] private DragonStats stats;
    [SerializeField] private DragonAnimator anim;
    [SerializeField] private DragonCombat combat;
    [SerializeField] private Rigidbody2D rb;


    public float chaseRange = 10f;
    public float rangeCanAttack1_2 = 2f;
    public float rangeCanAttack3 = 2f;
    public float jumpThreshold = 0f;

    public float attackCooldownRate = 0.3f;

    private float shoutCooldownTimer = 0f;  
    private float attackTimer01_2 = 0f;
    private float attackTimer03 = 0f;

    private float lastAttackTime01_2;
    private float lastAttackTime03;
    private float lastShoutTime;


    private float moveShakeCooldown = 0f;
    public float moveShakeInterval = 0.5f; // thời gian chờ giữa mỗi lần rung khi move
    public float moveShakeIntensity = 0.5f;
    public float moveShakeDuration = 0.2f;

    float distanceX;
    float distanceY;
    void Start()
    {
        // player = GameObject.FindWithTag("Player")?.transform;   
        stats.currentState = DragonState.Appear;
        combat.Appear();
    }

    void Update()
    {
        if (stats.currentState == DragonState.Die) return;

        distanceX = Mathf.Abs(player.position.x - (transform.position.x + stats.offset.x));
        distanceY = player.position.y - (transform.position.y + stats.offset.y);
        Vector2 dir = (Vector2)(player.position - transform.position).normalized;

        attackTimer01_2 -= Time.deltaTime;
        attackTimer03 -= Time.deltaTime;
        shoutCooldownTimer -= Time.deltaTime;

        bool isEnraged = stats.HPPercentage < 0.6f;
        float attackCooldown01_2 = stats.attackCooldown01_2;
        float attackCooldown03 = stats.attackCooldown03;
        float shoutCooldown = stats.shoutCooldown;
        if (isEnraged)
        {
            attackCooldown01_2 = (1 - attackCooldownRate) * attackCooldown01_2;
            attackCooldown03 = (1 - attackCooldownRate) * attackCooldown03;
            shoutCooldown = (1-attackCooldownRate) * shoutCooldown;
        }
        if(stats.currentState == DragonState.Appear)
        {
   //         stats.Flip(dir.x);
            return;
        }


        if (  stats.currentState == DragonState.Hurt || stats.currentState == DragonState.Shout
            || stats.currentState == DragonState.Attack)
            return;
        stats.Flip(dir.x);

        if(distanceX < rangeCanAttack3)// nếu trong phạm vi tấm công tầm xa thì:
        {
            // nếu ở trên mặt đất boss thì nhảy
            if (stats.IsGrounded) {
                if (distanceY > jumpThreshold)  // xét theo chiều dọc lại ngoài phạm vi thì:
                {
                    // Nhảy lên nếu player ở trên cao
                    stats.currentState = DragonState.Jump;
                    combat.Jump(player.position);
                    return;
                }
                // nếu ở trong phạm vi có thể tấn công xa nhất thì:
                else
                {
                    // kiểm tra thời gian hồi chiêu=> nếu có thể tấn công tầm xa thì tấn công tầm xa không thì xem xem có thể tấn công gần không , nếu có thì tấn công tầm gần, nếu không thì lại Shout cho vui
                    if(attackTimer03 <= 0f)
                    {
                        stats.currentState = DragonState.Attack;
                        stats.IsAttacking = true;
                        attackTimer03 = attackCooldown03;
                        combat.FireAttack(); 
                    }
                    else if (distanceX <= rangeCanAttack1_2)
                    {
                        if(attackTimer01_2 <= 0f)
                        {
                            stats.currentState = DragonState.Attack;
                            stats.IsAttacking = true;
                            attackTimer01_2 = attackCooldown01_2;
                            int index = Random.Range(1, 3);
                            AudioManager.Instance?.PlaySFX(stats.audioSource, stats.slashAttackSound);
                            anim.Attack(index); //cái này gắn sự kiện rồi do attack trong animation rồi
                        }
                        else if(shoutCooldownTimer<= 0f)
                        {
                            
                            combat.Shout();
                            shoutCooldownTimer = shoutCooldown;
                        }
                    }
                    else
                    {// đuổi
                        Chase(isEnraged);
                    }
                }
            }
            // không ở trên mặt đất thì:
            else
            {
                // tấn công nếu hết cooldown và trong phạm vi ( tầm ngắn)
                if (attackTimer01_2 <= 0f && distanceX < rangeCanAttack1_2)
                {
                    stats.currentState = DragonState.Attack;
                    stats.IsAttacking = true;
                    attackTimer01_2 = attackCooldown01_2;
                    int index = Random.Range(1, 3);
                    anim.Attack(index);
                }
            }
                
        }
        else
        {
            if (stats.IsGrounded) {
                Chase(isEnraged);
            }
        }


        if (Vector2.Distance(rb.position + stats.offset, player.transform.position) < 0.3f &&
            stats.currentState == DragonState.Jump)
        {
            rb.velocity = new Vector2(rb.velocity.x / 10f, rb.velocity.y);
        }
        if (rb.velocity.x != 0 && stats.IsGrounded)
        {
            if (moveShakeCooldown <= 0f)
            {
                CameraTrigger.Instance.ShakeCamera(moveShakeIntensity, moveShakeDuration);
                moveShakeCooldown = moveShakeInterval;
            }
        }
        moveShakeCooldown -= Time.deltaTime;
    }
    public void Chase(bool isEnraged)
    {
        stats.currentState = DragonState.Chase;
        if (isEnraged)
            combat.Run(player.position);
        else
            combat.Walk(player.position);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rb.position + stats.offset, rb.position + stats.offset + new Vector2(transform.localScale.x * distanceX, 0));

        Gizmos.DrawLine(rb.position + stats.offset, rb.position + stats.offset + new Vector2(0, distanceY));
        Gizmos.DrawWireSphere(rb.position + stats.offset, rangeCanAttack1_2);
        Gizmos.DrawWireSphere(rb.position + stats.offset, rangeCanAttack3);
    }
}
