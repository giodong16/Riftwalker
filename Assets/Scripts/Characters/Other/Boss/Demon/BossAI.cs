using UnityEngine;

public enum BossState { Idle, Chase, Attack, Jump, Hurt, Block }
[RequireComponent(typeof(Rigidbody2D))]
public class BossAI : MonoBehaviour
{
    public BossState currentState;
    public Transform player;
    [SerializeField] private BossStats stats;
    [SerializeField] private BossAnimation anim;
    [SerializeField] private BossCombat combat;
    [SerializeField] private Rigidbody2D rb;


    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float jumpThreshold = 1f;
/*    public float damageBlockThreshold = 40f; // tổng sát thương để bật khiên
    public float blockWindow = 3f; // trong vòng mấy giây

    private float recentDamage = 0f;
    private float damageTimer = 0f;
*/
    private float attackCooldown = 1.2f;
    private float attackTimer = 0f;

    [SerializeField] float baseAttackCooldown = 1.5f; //bình thường
    [SerializeField] float enragedAttackCooldown = 1f; // HP < 50%

    private float moveShakeCooldown = 0f;
    public float moveShakeInterval = 0.5f; // thời gian chờ giữa mỗi lần rung khi move
    public float moveShakeIntensity = 0.5f;
    public float moveShakeDuration = 0.2f;

    float distanceX;
    float distanceY;
    void Start()
    {
        // player = GameObject.FindWithTag("Player")?.transform;   
        currentState = BossState.Idle;

    }

    void Update()
    {
        if (stats.IsDead || !stats.IsCanController) return;
      
        bool isEnraged = stats.HPPercentage < 0.5f;
        attackCooldown = isEnraged ? enragedAttackCooldown : baseAttackCooldown;
    //    attackTimer -= Time.deltaTime;

        distanceX = Mathf.Abs(player.position.x - (transform.position.x+ stats.offset.x));
        distanceY = Mathf.Abs(player.position.y - (transform.position.y + stats.offset.y));
        Vector2 dir = (Vector2)(player.position - transform.position).normalized;
        stats.Flip(dir.x);
        // Nếu đang tấn công hoặc đang bị thương thì không làm gì cả
        if (stats.IsAttacking || currentState == BossState.Hurt) return;
        attackTimer -= Time.deltaTime;
        // Nếu trong phạm vi tấn công theo chiều ngang
        if (distanceX < attackRange)
        {
            if (stats.IsGrounded)
            {
                if (distanceY > jumpThreshold)  // theo chiều dọc lại ngoài phạm vi thì:
                {
                    // Nhảy lên nếu player ở trên cao
                    currentState = BossState.Jump;
                    anim.PlayJump();
                    combat.Jump(player.position);
                    return;
                }
                else // trong cả ngang lẫn dọc
                {
                    // Chỉ tấn công nếu hết cooldown
                    if (attackTimer <= 0f)
                    {
                        currentState = BossState.Attack;
                        stats.IsAttacking = true;
                        attackTimer = attackCooldown;

                        int index = Random.Range(1, 3);
                        anim.PlayAttack(index);
                      //  combat.DoAttack(index);
                    }
                    else
                    {
                        // Trong cooldown, không tấn công, chỉ idle
                        currentState = BossState.Chase;
                        if (isEnraged)
                            combat.Run(player.position); // <50%hp>
                        else
                            combat.Walk(player.position);
                    }
                }
            }
            else
            {
                // tấn công nếu hết cooldown
                if (attackTimer <= 0f)
                {
                    currentState = BossState.Attack;
                    stats.IsAttacking = true;
                    attackTimer = attackCooldown;

                    int index = Random.Range(1, 3);
                    anim.PlayAttack(index);
                   // combat.DoAttack(index);
                }
            }
            
        }
        else
        {
            // Đuổi theo
            currentState = BossState.Chase;
            if (isEnraged)
                combat.Run(player.position); // <50%hp>
            else
                combat.Walk(player.position);

            anim.PlayMove();
        }

        // làm chậm khi nhảy tới player
        if (Vector2.Distance(rb.position + stats.offset, player.transform.position) < 0.3f &&
            currentState == BossState.Jump)
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
    public void OnTakeDamage(float amount)
    {
        /*        recentDamage += amount;
                damageTimer = blockWindow;*/
        AudioManager.Instance?.PlaySFX(stats.audioSource, stats.hurtSound);
        anim.PlayeHurt();
        currentState = BossState.Hurt;
        stats.IsCanController = false;

     //   Invoke(nameof(EndHurt), 0.5f); // thời gian bị thương
    }

    private void Unblock()
    {
        combat.StopBlock();
        stats.IsBlocking = false;
    //    recentDamage = 0f;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rb.position+ stats.offset, rb.position + stats.offset +new Vector2(transform.localScale.x * distanceX,0));

        Gizmos.DrawLine(rb.position + stats.offset, rb.position + stats.offset + new Vector2(0,  distanceY));
        Gizmos.DrawWireSphere(rb.position+stats.offset,attackRange);
    }
}
