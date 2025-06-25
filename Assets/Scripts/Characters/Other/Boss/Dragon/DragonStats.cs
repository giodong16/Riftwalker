using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum DragonState { Appear, Idle, Chase, Attack, Jump, Hurt, Shout, Die }
public class DragonStats : MonoBehaviour
{
    public DragonState currentState;

    public Rigidbody2D rb;
    public float maxHP = 1000;
    public float walkSpeed = 2f;
    public float runSpeed = 8f;
    public float defense;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public Vector2 offset = new Vector2(0, 0);


    public Transform attackPoint01;
    public float attackRange01 = 0.5f;

    public Transform attackPoint02;
    public float attackRange02 = 0.5f;
    public LayerMask playerLayer;

    public float hurtEndTime;

    public float attackCooldown01_2 = 2f;
    public float attackCooldown03 = 10f;

    public float shoutCooldown = 2f;        
  



    public int damage = 200;

   // private float moveShakeCooldown = 0f;
    public float moveShakeInterval = 0.5f; // thời gian chờ giữa mỗi lần rung khi move
    public float moveShakeIntensity = 0.5f;
    public float moveShakeDuration = 0.2f;

    [Header("Sound")]
    public AudioClip awakeSound;
    public AudioClip jumpSound;
    public AudioClip moveSound;
    public AudioClip slashAttackSound;
    public AudioClip fireAttackSound;
    public AudioClip hurtSound;
    public AudioClip dieSound;
    public AudioClip shoutSound;
    public AudioSource audioSource;

    [Header("Particle System")]
    public GameObject stoneBrokenPrefab;
    public float currentHP;
    private bool isGrounded;
    private bool isAttacking;
    private bool isDead = false;
    private bool isCanController = true;
    public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public bool IsDead { get => isDead; set => isDead = value; }

    public bool IsCanController { get => isCanController; set => isCanController = value; }
    public float HPPercentage => currentHP / maxHP;

    private void Awake()
    {
        currentHP = maxHP;
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    public void Flip(float dirX)
    {
        if (dirX > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (dirX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(rb.position + offset, 0.1f);
        if (attackPoint01 != null)
            Gizmos.DrawWireSphere(attackPoint01.position, attackRange01);
        if (attackPoint02 != null)
            Gizmos.DrawWireSphere(attackPoint02.position, attackRange02);
    }
}
