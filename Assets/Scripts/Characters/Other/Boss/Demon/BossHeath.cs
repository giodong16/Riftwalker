using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHeath : MonoBehaviour
{
    public float defense;
    public FloatingHeathBarEnemy floatingHeathBarEnemy;
    public GameObject bloodPrefab;
    public Animator animator;
    public EnemyReward enemyReward;
    public BossStats stats;

    private void Start()
    {
        floatingHeathBarEnemy = GetComponent<FloatingHeathBarEnemy>();
        animator = GetComponent<Animator>(); 
    }

    public void TakeDamage(float damage, Vector3 position)
    {
        float totalDamaged = damage - defense;
        if (totalDamaged > 0)
        {
            DisplayDamageManage.Instance.ShowDamage((int)totalDamaged, position, Color.red);
            stats.currentHP -= totalDamaged;
            if (bloodPrefab != null)
            {
                Instantiate(bloodPrefab, position, Quaternion.identity);
            }
            if (animator != null && HasParameter(animator, "GetHit"))
            {
                animator.SetTrigger("GetHit");

            }
            GetComponent<BossAI>()?.OnTakeDamage(damage); // gọi AI để xử lý block/hurt
            if (floatingHeathBarEnemy != null)
            {
                floatingHeathBarEnemy.UpdateHeathBar(stats.currentHP, stats.maxHP);
            }

            if (stats.currentHP <= 0) DestroyWithEffect();
        }

    }
    bool HasParameter(Animator animator, string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }
    public void DestroyWithEffect()
    {
        stats.IsDead = true;
        EnemyLoot enemyLoot = GetComponent<EnemyLoot>();
        if (enemyLoot != null)
        {
            enemyLoot.DropLoot();
        }
        AudioManager.Instance?.PlaySFX(stats.audioSource, stats.dieSound);
        if (animator != null && HasParameter(animator, "Die"))
        {
            animator.SetTrigger("Die");
            Destroy(gameObject, 0.5f);
        }

        else
        {
            Destroy(gameObject, 0.2f);
        }
        if (enemyReward != null)
        {
           // enemyReward.parentTransform = transform;
            enemyReward.ShowRewards();
        }
    }



}
