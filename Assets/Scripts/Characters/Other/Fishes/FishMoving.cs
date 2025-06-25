using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] Vector2 _moveDirection = new Vector2(1f, 0.25f);
    [SerializeField] GameObject _rightCheck, _topCheck, _groundCheck;
    [SerializeField] Vector2 _rightCheckSize, _topCheckSize, _groundCheckSize;
    [SerializeField] LayerMask _borderLayer, _platform;
    [SerializeField] bool _isGoingUp = true;

    private bool _touchedGround, _touchedTop, _touchedRight;
    private Rigidbody2D _rb;
    private Animator _animator;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        HitLogic();
        HandleAnimation();
    }

    void FixedUpdate()
    {
        _rb.velocity = _moveDirection * _moveSpeed;
    }

    void HitLogic()
    {
        _touchedRight = HitDetector(_rightCheck, _rightCheckSize, (_borderLayer | _platform));
        _touchedTop = HitDetector(_topCheck, _topCheckSize, (_borderLayer | _platform));
        _touchedGround = HitDetector(_groundCheck, _groundCheckSize, (_borderLayer | _platform));

        if (_touchedRight)
        {
            Flip();
        }
        if (_touchedTop && _isGoingUp)
        {
            ChangeYDirection();
        }
        if (_touchedGround && !_isGoingUp)
        {
            ChangeYDirection();
        }
    }

    bool HitDetector(GameObject gameObject, Vector2 size, LayerMask layer)
    {
        return Physics2D.OverlapBox(gameObject.transform.position, size, 0f, layer);
    }

    void ChangeYDirection()
    {
        _moveDirection.y = -_moveDirection.y;
        _isGoingUp = !_isGoingUp;
    }

    void Flip()
    {
        transform.Rotate(new Vector2(0, 180));
        _moveDirection.x = -_moveDirection.x;
    }
    void HandleAnimation()
    {
        _animator.SetFloat("moveSpeed",_moveSpeed);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_groundCheck.transform.position, _groundCheckSize);
        Gizmos.DrawWireCube(_topCheck.transform.position, _topCheckSize);
        Gizmos.DrawWireCube(_rightCheck.transform.position, _rightCheckSize);
    }

}
