using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyCollider : MonoBehaviour
{
    bool isAttacked;
    public enum Damage
    {
        BasicEnemyDamage, MediumEnemyDamage, HighEnemyDamage
    }
    public Damage damageEnemy;
    int damage;
    private void Start()
    {
        switch (damageEnemy) { 
            case Damage.BasicEnemyDamage:
                damage = Pref.BasicEnemyDamage; break;
            case Damage.MediumEnemyDamage:
                damage = Pref.MediumEnemyDamage; break;
            case Damage.HighEnemyDamage:
                damage = Pref.HighEnemyDamage; break;
            default:
                damage = Pref.BasicEnemyDamage; break;
        }
    }
    Coroutine coroutine;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") &&!isAttacked)
        {
            isAttacked = true;
            Vector2 collisionPoint = collision.GetContact(0).point;
            PlayerController.Instance.TakeDamage(damage, collisionPoint);
            if(coroutine!= null)
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
