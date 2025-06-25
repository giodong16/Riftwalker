using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float delayBeforeFall = 0.5f;
    public float resetTime = 10f;
    public float resetDuration = 2f;
    public float shakeAmount = 0.05f;
    public float shakeDuration = 0.3f;
    public GameObject stonePrefab;

    private Rigidbody2D rb;
    private Vector3 originalPosition;
    private bool isFalling = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        originalPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFalling && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(StartFalling());
        }
        if(isFalling && collision.gameObject.CompareTag("Ground"))
        {
            Instantiate(stonePrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isFalling && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(StartFalling());
        }

    }
    IEnumerator StartFalling()
    {
        isFalling = true;

        // Rung nhẹ 
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            transform.position = originalPosition + (Vector3)Random.insideUnitCircle * shakeAmount;
            elapsed += Time.deltaTime;
            yield return null;
        }
        Instantiate(stonePrefab, transform.position, Quaternion.identity);
        transform.position = originalPosition; // trở lại vị trí đúng sau rung

        yield return new WaitForSeconds(delayBeforeFall);

        rb.isKinematic = false;

        yield return new WaitForSeconds(resetTime);

        StartCoroutine(ResetPlatform());
    }

    IEnumerator ResetPlatform()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;

        Vector3 startPosition = transform.position;
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            transform.position = startPosition + (Vector3)Random.insideUnitCircle * shakeAmount;
            elapsed += Time.deltaTime;
            yield return null;
        }

        Instantiate(stonePrefab, transform.position, Quaternion.identity);

        elapsed = 0f;

        while (elapsed < resetDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed/ resetDuration;
            transform.position = Vector3.Lerp(startPosition, originalPosition, t);
            yield return null;
        }

        transform.position = originalPosition;

        isFalling = false;
    }


}
