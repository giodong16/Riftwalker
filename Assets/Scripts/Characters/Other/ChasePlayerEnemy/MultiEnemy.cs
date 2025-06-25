using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiEnemy : MonoBehaviour
{

    public bool isStatic = false;
    public float moveSpeed = 3f;
    public Transform start;
    public Transform end;

    public Transform groundCheck;
    public float distanceGroundCheck;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public float distanceWallCheck;
    
    public float chaseSpeed = 3.5f;
    public float attackRange = 1.2f;
    public float detectionRange = 5f;
    public Transform attackTransform;
    public float hitboxRange = 1.2f;
    public Transform hitBoxTransform;
    public Transform player;
    public LayerMask playerLayer;
    public float cooldownTime = 1f;
    public float attackDamage = 200f;
    private bool _isMovingToEnd = true;
    public bool _isFacingRight = true;
    private bool _isPaused = false;
    private float _pauseTime = 1f;

    private bool _isAttack = false;
    private float lastAttackTime = -100f;
    public Vector2 target;
    private Rigidbody2D rb;
    private Animator anim;
    private bool canChase = false;

    public bool CanChase { get => canChase; set => canChase = value; }

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;    
        target = end.position;
    }

    private void Update()
    {
        if (isStatic) return;
        if(!canChase)
        {
            Patrol(); 
        }
        else
        {
            ChasePlayer();
        }
        anim.SetBool("isAttack", _isAttack);
    }

    private void ChasePlayer()
    {
        if (player == null || _isPaused) return;

        Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);

        // Kiểm tra hướng và lật mặt
        if ((targetPosition.x > transform.position.x  && !_isFacingRight) || (targetPosition.x < transform.position.x  && _isFacingRight))
        {
            Flip();
        }

        // Nếu gặp tường hoặc mép vực, dừng lại và sau đó quay lại tuần tra
        if (HitWall() || NearEdge())
        {
            StartCoroutine(PauseAndReturnToPatrol());
            return;
        }

        // Nếu không gặp chướng ngại vật, tiếp tục đuổi
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, chaseSpeed * Time.deltaTime);

        // Kiểm tra phạm vi tấn công
        if (Vector2.Distance(attackTransform.position, player.position) < attackRange && !_isAttack)
        {
            // AttackPlayer();
            if (CanAttack())
            {
                _isAttack = true;
                lastAttackTime = Time.time;
                anim.SetTrigger("Attack");
            }
        }
    }



    private void AttackPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(hitBoxTransform.position, hitboxRange, playerLayer);
        if (player != null)
        {
            Vector2 hitPosition = player.ClosestPoint(hitBoxTransform.position);
            PlayerController.Instance.TakeDamage(attackDamage, hitPosition);
        }

    }
    public void StopAttack() // dùng trong animation attack
    {
        _isAttack = false;
    }
    private bool CanAttack()  
    {
        return Time.time >= lastAttackTime + cooldownTime;
    }

    public void Patrol()
    {
        // Xác định hướng mục tiêu
        Vector2 direction = (target - (Vector2)transform.position).normalized;

        // Kiểm tra xem có cần lật mặt không
        if ((direction.x > 0 && !_isFacingRight) || (direction.x < 0 && _isFacingRight))
        {
            Flip();
        }

        // Di chuyển
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target) < 0.1f || HitWall() || NearEdge())
        {
            _isMovingToEnd = !_isMovingToEnd;
            target = _isMovingToEnd ? end.position : start.position;
        }
    }
    private IEnumerator PauseAndReturnToPatrol()
    {
        _isPaused = true; // Dừng di chuyển
        anim.SetBool("isAttack", false); // Dừng animation tấn công nếu có

        yield return new WaitForSeconds(_pauseTime); // Chờ trong khoảng thời gian

        _isPaused = false;
        CanChase = false;

        // Đặt lại target tuần tra gần nhất
        target = Vector2.Distance(transform.position, start.position) <
        Vector2.Distance(transform.position, end.position)? start.position : end.position;

        // Đảm bảo hướng quay lại đúng
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        if ((direction.x > 0 && !_isFacingRight) || (direction.x < 0 && _isFacingRight))
        {
            Flip();
        }
    }

    private bool NearEdge()
    {
        return !Physics2D.Raycast(groundCheck.position, Vector2.down, distanceGroundCheck, groundLayer);
    }
    private bool HitWall()
    {
        float direction = _isMovingToEnd ? 1f : -1f;
        return Physics2D.Raycast(transform.position, Vector2.right * direction, distanceWallCheck, wallLayer);
    }

    private void OnDrawGizmos()
    {
        // Vẽ raycast để debug
        Gizmos.color = Color.red;
        float direction = _isMovingToEnd ? 1f : -1f;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * direction * distanceWallCheck);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * distanceGroundCheck);
        Gizmos.DrawWireSphere(attackTransform.position, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitBoxTransform.position, hitboxRange);
    }
    public void Flip()
    {
        _isFacingRight = !_isFacingRight; 
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}
