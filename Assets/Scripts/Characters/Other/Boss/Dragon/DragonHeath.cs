using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonHeath : MonoBehaviour
{

    public FloatingHeathBarEnemy floatingHeathBarEnemy;
    public GameObject bloodPrefab;
    public Animator animator;
    public EnemyReward enemyReward;
    public DragonStats stats;
    public AnimationClip hurtClip;
    public float durationHurtTime;
    public float hurtTimer;

    private void Start()
    {
        floatingHeathBarEnemy = GetComponent<FloatingHeathBarEnemy>();
        durationHurtTime = hurtClip.length;
      //  animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (stats.currentState == DragonState.Hurt) { 
            hurtTimer -= Time.deltaTime;
            if (hurtTimer <= 0) {
                stats.currentState = DragonState.Idle;
            }
        }
        
    }
    public void TakeDamage(float damage, Vector3 position)
    {
        float totalDamaged = damage - stats.defense;
        if (totalDamaged > 0)
        {
            DisplayDamageManage.Instance.ShowDamage((int)totalDamaged, position, Color.red);
            stats.currentHP -= totalDamaged;

            if (bloodPrefab != null)
            {
                Instantiate(bloodPrefab, position, Quaternion.identity);
            }

            if (floatingHeathBarEnemy != null)
            {
                floatingHeathBarEnemy.UpdateHeathBar(stats.currentHP, stats.maxHP);
            }

            if (stats.currentHP <= 0)
            {
                DestroyWithEffect();
                return;
            }
            if (stats.currentState == DragonState.Hurt || stats.currentState == DragonState.Attack || stats.currentState == DragonState.Shout)
                return;

            // Nếu chưa ở trạng thái Hurt thì mới play animation và âm thanh
            if (animator != null)
            {

                stats.currentState = DragonState.Hurt;
                hurtTimer = durationHurtTime;
                animator.Play("Hurt");
                AudioManager.Instance?.PlaySFX(stats.audioSource, stats.hurtSound);
            }
            /*if (stats.currentState == DragonState.Attack || stats.currentState == DragonState.Shout )
            {
                return;
            }

            if (animator != null)
            {
                stats.currentState = DragonState.Hurt;
                animator.Play("Hurt", 0, 0f); //  phát lại từ đầu
                AudioManager.Instance?.PlaySFX(stats.audioSource, stats.hurtSound);

                // lưu lại thời gian kết thúc Hurt (phòng tránh TurnOnController gọi sai)
                float animLength = animator.GetCurrentAnimatorStateInfo(0).length;
                stats.hurtEndTime = Time.time + animLength;
            }*/
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
        stats.currentState = DragonState.Die;
        EnemyLoot enemyLoot = GetComponent<EnemyLoot>();
        if (enemyLoot != null)
        {
            enemyLoot.DropLoot();
        }
        AudioManager.Instance?.PlaySFX(stats.audioSource, stats.dieSound);
        if (animator != null)
        {
            stats.currentState = DragonState.Die;
            animator.Play("Die");
            Destroy(gameObject, 0.5f);
        }

        else
        {
            Destroy(gameObject, 0.5f);
        }
        if (enemyReward != null)
        {
            enemyReward.ShowRewards();
        }
    }

}
