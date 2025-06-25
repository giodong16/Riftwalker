using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrap : MonoBehaviour
{
    public Rigidbody2D rb;
    public float timeDelay = 0.5f;
    private bool isFalled = false;

    public Transform groundCheck;
    public float distanceCheck = 0.3f;
    public LayerMask groundLayer;
    public Collider2D col;
    bool isDestroy = false;
    private void Update()
    {
        if (isDestroy) return;
        bool hitGround = Physics2D.Raycast(groundCheck.position, Vector2.down, distanceCheck, groundLayer);
        if(hitGround)
        {
            DestroyEffect();
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !isFalled)
        {
            isFalled = true;
            Invoke("DropSpike", timeDelay);
        }
    }

    void DropSpike()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.mass = 5;
    }
    public void DestroyEffect()
    {
        isDestroy = true;
        col.isTrigger = false;
        rb.constraints = RigidbodyConstraints2D.None;
        Destroy(gameObject, 1f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * distanceCheck);
    }
}
