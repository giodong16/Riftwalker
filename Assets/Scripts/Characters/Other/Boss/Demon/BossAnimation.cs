using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public BossStats stats;
    public BossAI bossAI;
    public BossCombat combat;

    public Transform attackPoint01;
    public float attackRange01 = 0.5f;

    public Transform attackPoint02;
    public float attackRange02 = 0.5f;
    private void Update()
    {
        animator.SetBool("IsGrounded", stats.IsGrounded);
        animator.SetFloat("velocityX", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("velocityY", rb.velocity.y);
        //  PlayMove();
        animator.SetBool("IsAttacking",stats.IsAttacking);
        animator.SetBool("isDead", stats.IsDead);
        animator.SetBool("IsBlocking",stats.IsBlocking);
        animator.SetBool("CanController",stats.IsCanController);
    }
    public void PlayMove()
    {
        animator.SetFloat("velocityX",Mathf.Abs(rb.velocity.x));

    }
    public void PlayAttack(int type)
    {
        animator.SetInteger("AttackType", type);
        animator.SetTrigger("Attack");
        //combat.DoAttack(type);
    }
    public void DoAtack(int type)
    {
        combat.DoAttack(type);
    }
    public void PlayJump() 
    {
        animator.SetFloat("velocityY", rb.velocity.y);
        //animator.SetTrigger("jump"); 
    }
    public void PlayDeath()
    { 
        animator.SetBool("isDead", true); 
    }
    public void PlayeHurt()
    {
        animator.SetTrigger("GetHit");
    }
    public void StopAttack()
    {
        
        stats.IsAttacking = false;
    //    Debug.Log("StopAttack được gọi");
    }
    public void TurnOnController()
    {
        bossAI.currentState = BossState.Idle;
        stats.IsCanController = true;
    }
    public void TurnOnBlock()
    {
        animator.SetBool("IsBlocking",true);
    }
    public void StopBlock()
    {
        stats.IsBlocking = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(rb.position + stats.offset, 0.1f);
        if (attackPoint01 != null)
            Gizmos.DrawWireSphere(attackPoint01.position, attackRange01);
        if (attackPoint02 != null)
            Gizmos.DrawWireSphere(attackPoint02.position, attackRange02);
    }
}
