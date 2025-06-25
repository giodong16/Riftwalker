using UnityEngine;

public class ChargeAttack : MonoBehaviour
{
    public float chargeSpeed = 10f;
    public float chargeDuration = 0.5f;
    public int damage = 20;
    public LayerMask playerLayer;
    public BossStats stats;

    public Rigidbody2D rb;
    private bool isCharging = false;
    private float chargeTimer;

    private void Update()
    {
        if (isCharging)
        {
            chargeTimer -= Time.deltaTime;
            if (chargeTimer <= 0)
            {
                StopCharge();
            }
        }
    }

    public void StartCharge(Vector2 direction)
    {
        isCharging = true;
        chargeTimer = chargeDuration;
        rb.velocity = direction.normalized * chargeSpeed;
    }

    private void StopCharge()
    {
        isCharging = false;
        rb.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isCharging) return;
        Vector2 hitPoint = collision.GetContact(0).point;
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            PlayerController.Instance.TakeDamage(stats.damage*5, hitPoint);
            StopCharge();
        }
        else
        {
            // Gặp tường hoặc chắn
            StopCharge();
        }
    }
}
