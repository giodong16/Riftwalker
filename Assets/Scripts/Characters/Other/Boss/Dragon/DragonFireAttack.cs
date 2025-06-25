using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragonFireAttack : MonoBehaviour
{
    public bool isAttack = false;
    private Coroutine coroutine;
    public DragonStats stats;
    public float damageInterval = 0.2f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 hitPos = collision.ClosestPoint(transform.position);
            coroutine = StartCoroutine(DealDamageOverTime(hitPos));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopCoroutine(coroutine);
        }
    }

    IEnumerator DealDamageOverTime(Vector2 hitPosition)
    {
        while (true) {

            PlayerController.Instance?.TakeDamage(stats.damage, hitPosition);
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
