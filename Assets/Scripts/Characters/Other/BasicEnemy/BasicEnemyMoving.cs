using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyMoving : MonoBehaviour
{
    public bool isStatic = false;
    public float moveSpeed = 3f;
    public Transform start;
    public Transform end;

    public Transform groundCheck;
    public float distanceGroundCheck =0.5f;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public float distanceWallCheck = 0.5f;

    private SpriteRenderer _spriteRenderer;
    private bool _isMovingToEnd = true;
    private Vector2 target;


    private void Start()
    {   
        _spriteRenderer = GetComponent<SpriteRenderer>();
        target = end.position;
    }

    private void Update()
    {
        if(isStatic) return;
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed* Time.deltaTime);
       
        if(Vector2.Distance(transform.position, target) < 0.1f || HitWall() || NearEdge())
        {
            _isMovingToEnd =!_isMovingToEnd;
            target = _isMovingToEnd ? end.position : start.position;
            _spriteRenderer.flipX = !_isMovingToEnd;
        }      
    }

    private bool NearEdge()
    {
        return !Physics2D.Raycast(groundCheck.position, Vector2.down, distanceGroundCheck, groundLayer);
    }
    private bool HitWall()
    {
        float direction = _isMovingToEnd ? 1f : -1f;
        return Physics2D.Raycast(transform.position,Vector2.right* direction,distanceWallCheck,wallLayer);
    }

    private void OnDrawGizmos()
    {
        // Vẽ raycast để debug
        Gizmos.color = Color.red;
        float direction = _isMovingToEnd ? 1f : -1f;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * direction * distanceWallCheck);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * distanceGroundCheck);
    }
}
