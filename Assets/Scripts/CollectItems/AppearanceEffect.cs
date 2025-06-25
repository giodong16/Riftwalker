using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearanceEffect : MonoBehaviour
{
    public float moveUpDistance = 0.5f;
    public float durationTime = 0.5f;

    public void Appearances()
    {
        StartCoroutine(UpEffect());
    }

    IEnumerator UpEffect()
    {
        float elapsed = 0f;
        Vector3 start = transform.position;
        Vector3 end = transform.position + Vector3.up * moveUpDistance;
        while (elapsed < durationTime)
        {
            float t = elapsed / durationTime;
            transform.position = Vector3.Lerp(start, end, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

    }
}
