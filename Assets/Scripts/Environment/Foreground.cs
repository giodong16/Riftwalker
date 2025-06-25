using System.Collections;
using UnityEngine;

public class Foreground : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Coroutine _fadeOutCoroutine;
    [SerializeField] private float _durationTime = 0.2f;
    [SerializeField] private float _minAlpha = 200f/255f;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Kiểm tra nếu đang có một Coroutine fade-out chạy và dừng nó
            if (_fadeOutCoroutine != null)
            {
                StopCoroutine(_fadeOutCoroutine);
            }

            // Lấy màu alpha hiện tại và bắt đầu quá trình fade
            _fadeOutCoroutine = StartCoroutine(FadeAlpha(1f, _minAlpha)); // Alpha = 200/255
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Kiểm tra nếu đang có một Coroutine fade-in chạy và dừng nó
            if (_fadeOutCoroutine != null)
            {
                StopCoroutine(_fadeOutCoroutine);
            }

            if (gameObject.activeInHierarchy)
              // Lấy màu alpha hiện tại và bắt đầu quá trình fade
                _fadeOutCoroutine = StartCoroutine(FadeAlpha(_minAlpha, 1f)); // Alpha = 255/255
        }
    }

    private IEnumerator FadeAlpha(float startAlpha, float endAlpha)
    {
        float timeCount = 0f;
        Color color = _spriteRenderer.color;
        color.a = startAlpha;
        _spriteRenderer.color = color;

        // Sử dụng `Mathf.Lerp` để thay đổi alpha một cách mượt mà
        while (timeCount < _durationTime)
        {
            timeCount += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, timeCount / _durationTime);
            color.a = newAlpha;
            _spriteRenderer.color = color;
            yield return null;
        }

        // Đảm bảo rằng màu đã đạt đến giá trị cuối cùng
        color.a = endAlpha;
        _spriteRenderer.color = color;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
