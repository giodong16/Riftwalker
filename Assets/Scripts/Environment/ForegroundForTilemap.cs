using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ForegroundForTilemap : MonoBehaviour
{
    private Tilemap _tilemap;
    private Coroutine _fadeCoroutine;
    [SerializeField] private float _durationTime = 0.2f;
    [SerializeField] private float _minAlpha = 200f / 255f;

    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_fadeCoroutine != null)
                StopCoroutine(_fadeCoroutine);

            _fadeCoroutine = StartCoroutine(FadeAlpha(1f, _minAlpha));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_fadeCoroutine != null)
                StopCoroutine(_fadeCoroutine);

            if (gameObject.activeInHierarchy)
                _fadeCoroutine = StartCoroutine(FadeAlpha(_minAlpha, 1f));
        }
    }

    private IEnumerator FadeAlpha(float startAlpha, float endAlpha)
    {
        float timeCount = 0f;
        Color color = _tilemap.color;
        color.a = startAlpha;
        _tilemap.color = color;

        while (timeCount < _durationTime)
        {
            timeCount += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, timeCount / _durationTime);
            color.a = newAlpha;
            _tilemap.color = color;
            yield return null;
        }

        color.a = endAlpha;
        _tilemap.color = color;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
