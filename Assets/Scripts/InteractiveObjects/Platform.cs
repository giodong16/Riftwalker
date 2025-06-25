using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Platform : MonoBehaviour
{
    public bool isStatic = false;
    public float moveSpeed = 3f;
    public Transform start;
    public Transform end;

    public float distanceGroundCheck = 0.5f;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public float distanceWallCheck = 0.5f;

    private Transform target;
    private Rigidbody2D rb;  

    private void Start()
    {
        target = end;
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void FixedUpdate() 
    {
        if (isStatic) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        if (Vector2.Distance(transform.position, target.position) < 0.1f || HitWall() || HitGround())
        {
            target = target == start ? end : start;
            direction = (target.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
    }

    private bool HitGround()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, distanceGroundCheck, groundLayer) ||
               Physics2D.Raycast(transform.position, Vector2.up, distanceGroundCheck, groundLayer);
    }

    private bool HitWall()
    {
        return Physics2D.Raycast(transform.position, Vector2.right, distanceWallCheck, wallLayer) ||
               Physics2D.Raycast(transform.position, Vector2.left, distanceWallCheck, wallLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * distanceWallCheck);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * distanceGroundCheck);
    }
}
