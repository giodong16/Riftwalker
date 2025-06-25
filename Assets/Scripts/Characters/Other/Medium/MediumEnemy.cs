using UnityEngine;

public class MediumEnemy : MonoBehaviour
{
    public float detectRange = 5f;
    public float moveSpeed = 5f;
    public float jumpPower = 4f;
    public Transform player;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float distanceGroundCheck = 0.1f;
    public Animator animator;
    public bool detectedPlayer = false;
    

    private bool isPassiveState = false;
    bool isFacingRight = false;
    private Rigidbody2D rb;
    private float currentJumpPower;
    private bool canJump = true;
    private bool isGrounded;
    private AudioSource audioSource;

    private float jumpCooldown = 0.8f; // thời gian giữa các cú nhảy

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isPassiveState) return;

        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, distanceGroundCheck, groundLayer);

        if (detectedPlayer)
        {
            if (isGrounded && canJump)
            {
                AudioManager.Instance?.PlaySFXLoop(audioSource, audioSource.clip);
                JumpTowardPlayer();

            }
        }
        else
        {
            AudioManager.Instance?.StopSFXLoop(audioSource);
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.Play("Idle");

        }

        animator.SetBool("Moving", detectedPlayer);
    }
    private void JumpTowardPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        Flip(direction);

        currentJumpPower = jumpPower;
        rb.velocity = new Vector2(direction.x * moveSpeed, currentJumpPower);

        canJump = false;
        Invoke(nameof(ResetJump), jumpCooldown);
    }

    private void ResetJump()
    {
        canJump = true;
    }



    public void Flip(Vector2 direction) {
        if ((direction.x < 0 && isFacingRight) || (direction.x > 0 && !isFacingRight)) {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    public void TurnOnPassiveState()
    {
        isPassiveState = true;
    }

    public void EndPassiveState()
    {
        isPassiveState = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * distanceGroundCheck);
    }
}
