using UnityEngine;

public class EnemyMultiShooter : MonoBehaviour
{
    public GameObject bulletBallPrefab;
    public Transform[] firePoints;       
    public float bulletSpeed = 10f;
    public float shootSpeed = 2f;

    private float shootTimer;

    void Update()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootSpeed)
        {
            Shoot();
            shootTimer = 0f;
        }
    }

    void Shoot()
    {
        foreach (Transform point in firePoints)
        {
            float angle = Random.Range(0f, 360f);
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

            GameObject bulletBall = Instantiate(bulletBallPrefab, point.position, Quaternion.identity);
            Rigidbody2D rb = bulletBall.GetComponent<Rigidbody2D>();
            rb.velocity = dir * bulletSpeed;
        }
    }
}
