using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInPlatform : MonoBehaviour
{
    /*public LayerMask platformLayer;
    public float rayLength = 0.1f; // Độ dài tia raycast

    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform playerTransform;

    private Transform currentPlatform; // Lưu trữ platform hiện tại

    void Update()
    {
        // Kiểm tra xem có platform bên dưới không
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, platformLayer);
        if (hit.collider != null)
        {
            currentPlatform = hit.collider.transform; // Gán platform hiện tại
            playerTransform.SetParent(currentPlatform);
        }
        else
        {
            playerTransform.SetParent(null);
        }
    }*/

/*    private void OnDrawGizmos()
    {
        // Vẽ ray để dễ debug
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayLength);
    }*/
}
