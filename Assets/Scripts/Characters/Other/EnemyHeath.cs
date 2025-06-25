using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHeath : MonoBehaviour
{
    public float maxHP = 500;
    public float defense;
    public FloatingHeathBarEnemy floatingHeathBarEnemy;
    public GameObject bloodPrefab;
    public Animator animator;
    public EnemyReward enemyReward;
    private float currentHP;

    private void Start()
    {
        floatingHeathBarEnemy = GetComponent<FloatingHeathBarEnemy>();
        animator = GetComponent<Animator>();
        currentHP = maxHP;
    }

    public void TakeDamage(float damage, Vector3 position)
    {
        float totalDamaged = damage - defense;
        if (totalDamaged > 0) {
            DisplayDamageManage.Instance.ShowDamage((int)totalDamaged, position, Color.red);
            currentHP -= damage;
            if (bloodPrefab != null)
            {
                Instantiate(bloodPrefab, position,Quaternion.identity);
            }
            if(animator != null && HasParameter(animator,"GetHit"))
            {
                animator.SetTrigger("GetHit");

            }
            if (floatingHeathBarEnemy != null) { 
                floatingHeathBarEnemy.UpdateHeathBar(currentHP,maxHP);
            }

            if (currentHP <= 0) DestroyWithEffect();
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
        EnemyLoot enemyLoot = GetComponent<EnemyLoot>();
        if (enemyLoot != null) {
            enemyLoot.DropLoot();
        }
        if (animator != null && HasParameter(animator, "Die"))
        {
            animator.SetTrigger("Die");
            Destroy(gameObject, 0.5f);
        }

        else
        {
            Destroy(gameObject,0.2f);
        }
        if (enemyReward != null) { 
            enemyReward.ShowRewards();
        }
    }



}
