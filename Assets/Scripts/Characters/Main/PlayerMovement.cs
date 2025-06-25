using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController playerController;
    private AudioManager audioManager;
    [Header("Move")]
    public Rigidbody2D _rb;
   // private bool _canMove = true;
    private bool _canFlip = true;
    private bool _canJump = true;


    private float _horizontalMove;
    private float _verticalMove;

    private bool _isGrounded;
    private bool _isFacingRight = true;
    private bool _isRunning;

    private int _jumpCount = 0;
    private float _dashTimeLeft;
    private float _lastImageXpos;
    [SerializeField] private AttackData dashData;
    private bool _isDashing;
    private bool _canDash = true;
    [SerializeField] private float _distanceBetweenImages;


    //sliding 
    [SerializeField] private float _slidingDurationTime = 0.5f;
    private bool _isSliding;


    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundCheckRadius = 0.2f;

    [Header("Wall Check")]
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private float _wallCheckRadius = 0.2f;

   

    [SerializeField] private Transform playerTransform;

    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private LayerMask bucketLayer;
    [SerializeField] private float rayLength = 0.1f; // Độ dài tia raycast
    [SerializeField] private Transform platformCheck;
    private Rigidbody2D platformRb;
    private Transform currentPlatform;
    private Transform currentBucket;
    private bool isOnPlatform = false;
    private bool isOnBucket = false;


    public PlatformType platformType = PlatformType.None;

    public bool IsGrounded { get => _isGrounded; set => _isGrounded = value; }
    public bool IsRunning { get => _isRunning; set => _isRunning = value; }
    public bool IsSliding { get => _isSliding; set => _isSliding = value; }
    public bool CanDash { get => _canDash; set => _canDash = value; }
    public bool CanJump { get => _canJump; set => _canJump = value; }
    public bool IsFacingRight { get => _isFacingRight; set => _isFacingRight = value; }
    public float HorizontalMove { get => _horizontalMove; set => _horizontalMove = value; }
    public float VerticalMove { get => _verticalMove; set => _verticalMove = value; }

    private void Start()
    {
        playerController = PlayerController.Instance;
        audioManager = AudioManager.Instance;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void Update()
    {

        GroundCheck();
        CheckMoveOnPlatform();
      //  CheckBucket();
        HandleFlip();
        CheckDash();
    }
    public void FixedUpdate()
    {
        if (playerController.CanController)
        {
            _rb.velocity = new Vector2(_horizontalMove *playerController.currentMoveSpeed, _rb.velocity.y);

            if (isOnPlatform && platformRb !=null&& platformRb.velocity.y < 0.2f )
            {
                _rb.velocity += platformRb.velocity;
            }
        }
    }
    public void HandleFlip()
    {
        if (_canFlip)
        {
            Flip();
        }
    }

    public void Flip()
    {
        if (_isFacingRight && _horizontalMove < 0f || !_isFacingRight && _horizontalMove > 0f)
        {
            _isFacingRight = !_isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    public void GroundCheck()
    {
        if (Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer))
        {

            _isGrounded = true;
            _jumpCount = 0;
        }
        else
        {
            _isGrounded = false;
        }
    }
    public bool IsWalled()
    {
        return Physics2D.OverlapCircle(_wallCheck.position, _wallCheckRadius, _wallLayer);
    }
    public void Move(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        // Cho phép reset về 0 khi không điều khiển
        if (!playerController.CanController && input != Vector2.zero) return;
        _horizontalMove = input.x;
        _verticalMove = input.y;
        
    }


    public void Jump(InputAction.CallbackContext context) {
        if (!playerController.CanController) return;
        if (_canJump && _jumpCount < 2) {
            if (context.performed)
            {
                _jumpCount++;
                
                _rb.velocity = new Vector2(_rb.velocity.x, playerController.currentJumpPower);
            }
            else if (context.canceled) {
                _rb.velocity = new Vector2(_rb.velocity.x, playerController.currentJumpPower * 0.5f);

            }
        }   
    }

    public void HandleSliding(InputAction.CallbackContext context)
    {
        if (!playerController.CanController) return;
        if (context.performed)
        {
            if (_isGrounded && _isRunning && !_isSliding)
            {
                _isSliding = true;
                audioManager?.PlaySFXLoop(playerController.audioSource, playerController.slidingSound);
                StartCoroutine(StopSliding());
            }
        }
        else if (context.canceled)
        {
            _isSliding = false;
        }
            
    }
    IEnumerator StopSliding()
    {
        yield return new WaitForSeconds(_slidingDurationTime);
        _isSliding = false;
    }

    public void HandleDash(InputAction.CallbackContext context)
    {
        if (!playerController.CanController) return;
        if (context.performed)
        {
            if (_canDash)
            {
                float cooldownTime = dashData.cooldownTime * (1 - playerController.CurrentStats.cooldownRate);
                if (Time.time >= (dashData.lastAttackTime + cooldownTime))
                {
                    _isDashing = true;
                    _dashTimeLeft = playerController.currentDashTime;
                    dashData.lastAttackTime = Time.time;
                    audioManager?.PlaySFX(playerController.audioSource,playerController.dashSound);

                    UIManager.Instance?.UpdateCooldown(NameAttack.Dash, cooldownTime);
                    PlayerAfterImagePool.Instance.GetFromPool();
                    _lastImageXpos = playerTransform.position.x;
                }
            }
            
        }

        CheckDash();
    }
    private void CheckDash()
    {
        if (_isDashing)
        {
            if (_dashTimeLeft > 0)
            {
                playerController.CanController = false;
                _canFlip = false;

                _rb.velocity = new Vector2(playerController.currentDashSpeed * playerTransform.localScale.x, _rb.velocity.y);
                _dashTimeLeft -= Time.deltaTime;

                if (MathF.Abs(playerTransform.position.x - _lastImageXpos) > _distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    _lastImageXpos = playerTransform.position.x;
                }
            }
            if (_dashTimeLeft <= 0 || IsWalled())
            {
                _isDashing = false;
                _canFlip = true;
                playerController.CanController = true;
            }
        }
    }


    #region CheckPlatform
    private void CheckMoveOnPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(platformCheck.position, Vector2.down, rayLength, platformLayer);
        if (hit.collider != null)
        {
            isOnPlatform = true;
            platformRb = hit.collider.GetComponent<Rigidbody2D>();
        }
        else
        {

            isOnPlatform = false;
            platformRb = null;
        }
    }
    /*private void CheckMoveOnPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(platformCheck.position, Vector2.down, rayLength, platformLayer);
        if (hit.collider != null)
        {
            isOnPlatform = true;
            platformRb = hit.collider.GetComponent<Rigidbody2D>();

            PlatformId id = hit.collider.GetComponent<PlatformId>();
            if (id != null)
            {
                platformType = id.platformType;
            }
            else
            {
                platformType = PlatformType.Linear; // Mặc định
            }
        }
        else
        {
            isOnPlatform = false;
            platformRb = null;
            platformType = PlatformType.None;
        }
    }*/

    private void CheckBucket()
    {
        RaycastHit2D hit = Physics2D.Raycast(platformCheck.position, Vector2.down, rayLength, bucketLayer);
        if (hit.collider != null)
        {
            isOnBucket = true;

            Transform bucketTransform = hit.collider.transform;

            if (currentBucket != bucketTransform)
            {
                transform.SetParent(bucketTransform);
                currentBucket = bucketTransform;
            }
        }
        else
        {
            if (isOnBucket && currentBucket != null)
            {
                transform.SetParent(null);
                currentBucket = null;
            }

            isOnBucket = false;
        }
    }
    private void OnDrawGizmos()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
        Gizmos.DrawWireSphere(_wallCheck.position, _wallCheckRadius);
      
        Gizmos.color = Color.red;
        Gizmos.DrawLine(platformCheck.position, platformCheck.position + Vector3.down * rayLength);
        Gizmos.color = Color.green;
       
    }
    #endregion
}
