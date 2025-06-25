using UnityEngine;

public class BossStats : MonoBehaviour
{
    public float maxHP = 1000;
    public float walkSpeed = 2f;
    public float runSpeed = 6f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public Vector2 offset = new Vector2(0,-2);

 //   public float attackCooldown = 2f;
    private float lastAttackTime;
    public int damage = 200;

    public AudioClip awakeSound;
    public AudioClip jumpSound;
    public AudioClip moveSound;
    public AudioClip attackSound;
    public AudioClip hurtSound;
    public AudioClip dieSound;
    public AudioSource audioSource;

    [HideInInspector] public float currentHP;
    private bool isGrounded;
    private bool isAttacking;
    private bool isDead = false;
    private bool isBlocking = false;
    private bool isCanController = true;
    public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public bool IsDead { get => isDead; set => isDead = value; }
    public bool IsBlocking { get => isBlocking; set => isBlocking = value; }
    public bool IsCanController { get => isCanController; set => isCanController = value; }
    public float HPPercentage => currentHP / maxHP;
    void Start()
    {
        currentHP = maxHP;
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(awakeSound);
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    public void Flip(float dirX)
    {
        if (dirX > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (dirX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }
}
