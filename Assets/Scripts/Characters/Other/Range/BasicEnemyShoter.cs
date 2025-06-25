using System.Collections;
using UnityEngine;

public class BasicEnemyShooter : MonoBehaviour
{
    public GameObject bulletBallPrefab;
    public Transform firePoint;    
    public float spawnSpeed = 2f;   
    public float ballSpeed = 10f;
    public bool shootRight = true;
    public float delayShoot = 0;
    public Animator animator;
    private float shootTimer;

    

    void Update()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= spawnSpeed)
        {
            StartCoroutine(Shoot());
            shootTimer = 0f;
        }
    }

    IEnumerator Shoot()
    {
        Vector2 direction = shootRight ? Vector2.right : Vector2.left;

        GameObject stoneBall = Instantiate(bulletBallPrefab, firePoint.position, Quaternion.identity);
        yield return new WaitForSeconds(delayShoot);
        if (animator != null)
        {
            animator.Play("Attack");
        }
        Rigidbody2D rb = stoneBall.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * ballSpeed;
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
}
