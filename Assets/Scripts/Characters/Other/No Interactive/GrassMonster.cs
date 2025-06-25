using System.Collections;
using UnityEngine;

public class GrassMonster : MonoBehaviour
{
    public float speed = 2f;
    private Transform spawnPoint;
    private Transform endPoint;
    private Vector2 target;
    private bool reachedFirstTime = false;
    private GrassMonsterSpawner spawner;
    private SpriteRenderer spriteRenderer;

    public void Initialize(Transform spawn, Transform end, GrassMonsterSpawner spawnerRef)
    {
        spawnPoint = spawn;
        endPoint = end;
        target = endPoint.position;
        spawner = spawnerRef;
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSpriteDirection();
    }

    void Update()
    {
        if (target == null) return;

        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target) < 0.05f)
        {
            if (!reachedFirstTime)
            {
                spawner.StopSpawning();
                reachedFirstTime = true;
            }

            StartCoroutine(ResetAndMoveAgain());
        }
    }

    IEnumerator ResetAndMoveAgain()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 1));

        transform.position = spawnPoint.position;
        target = endPoint.position;

        UpdateSpriteDirection();
    }

    void UpdateSpriteDirection()
    {
        if (spriteRenderer == null) return;

        if (target.x > transform.position.x)
            spriteRenderer.flipX = true; // Đi sang phải
        else
            spriteRenderer.flipX = false;  // Đi sang trái
    }
}
