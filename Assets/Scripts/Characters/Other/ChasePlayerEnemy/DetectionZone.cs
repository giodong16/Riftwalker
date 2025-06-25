using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    private MultiEnemy enemy;
    private void Start()
    {
        enemy = GetComponentInParent<MultiEnemy>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemy.CanChase = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemy.CanChase = false;

            // Đặt lại target tuần tra gần nhất
            enemy.target = Vector2.Distance(enemy.transform.position, enemy.start.position) <
                           Vector2.Distance(enemy.transform.position, enemy.end.position)
                           ? enemy.start.position
                           : enemy.end.position;

            // Đảm bảo hướng quay lại đúng
            Vector2 direction = (enemy.target - (Vector2)enemy.transform.position).normalized;
            if ((direction.x > 0 && !enemy._isFacingRight) || (direction.x < 0 && enemy._isFacingRight))
            {
                enemy.Flip();
            }
        }
    }
}
