using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class DragonAnimator : MonoBehaviour
{
    public DragonStats stats;
    public DragonCombat combat;
    Animator animator;
    
    public AnimationClip appearAnimation;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        animator.SetBool("IsGrounded", stats.IsGrounded);
        animator.SetFloat("velocityX", Mathf.Abs(stats.rb.velocity.x));
       
        //  PlayMove();
        animator.SetBool("IsAttacking", stats.IsAttacking);
      //  animator.SetBool("CanController", stats.IsCanController);
    }
    public void Appear()
    {
        animator.Play("Appear");
    }
    public void Idle()
    {
        stats.currentState = DragonState.Idle;
        animator.Play("Idle");
    }
    public void Attack(int type)
    {
        animator.Play("Attack0" + type.ToString());
    }
    public void DoAtack(int type)// cho 1 với 2 thôi
    {
        combat.DoAttack(type);
    }

    public void PlayeHurt()
    {
        stats.currentState = DragonState.Hurt;
        animator.Play("Hurt");
    }
    public void StopAttack()
    {
        stats.IsAttacking = false;
        stats.currentState = DragonState.Idle;
        //    Debug.Log("StopAttack được gọi");
        stats.audioSource.Stop();
    }
    public void Shout()
    {
        stats.currentState = DragonState.Shout;
        AudioManager.Instance.PlaySFX(stats.audioSource, stats.shoutSound);
        animator.Play("Shout");
        
    }
    public void TurnOnController()
    {
        if (stats.currentHP <= 0 || stats.currentState == DragonState.Die) return;

        
  //      if (Time.time < stats.hurtEndTime) return;

        stats.currentState = DragonState.Idle;
        stats.audioSource.Stop();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(combat.rb.position + stats.offset, 0.1f);
        if (stats.attackPoint01 != null)
            Gizmos.DrawWireSphere(stats.attackPoint01.position, stats.attackRange01);
        if (stats.attackPoint02 != null)
            Gizmos.DrawWireSphere(stats.attackPoint02.position, stats.attackRange02);
    }
}
