using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    int damage = 9999;
    bool isAttacked;
    Coroutine coroutine;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathZone") && !isAttacked)
        {
            isAttacked = true;
            PlayerController.Instance?.TakeDamage(damage);
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(ResetAttack());

        }
    }
    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.5f);
        isAttacked = false;
    }

}
