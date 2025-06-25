using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Image fillImage;
    private Coroutine currentCoroutine;

    private void OnEnable()
    {
        if (PlayerController.Instance != null)
        {
            UpdateHPBar( 0, true);
        }
    }
    public void UpdateHPBar(float durationTime, bool isInstant = false)
    {
        if (PlayerController.Instance == null|| PlayerController.Instance.MaxHP <= 0 || fillImage == null) return;

        float targetFill = Mathf.Clamp01(PlayerController.Instance.CurrentHP / PlayerController.Instance.MaxHP);

        if (isInstant)
        {
            fillImage.fillAmount = targetFill;
        }
        else
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);

            currentCoroutine = StartCoroutine(AnimateFill(targetFill, durationTime));
        }
    }

    private IEnumerator AnimateFill(float targetFill, float durationTime)
    {
        float elapsedTime = 0f;
        float startFill = fillImage.fillAmount;

        while (elapsedTime < durationTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsedTime / durationTime);
            fillImage.fillAmount = Mathf.Lerp(startFill, targetFill, t);
            yield return null;
        }

        fillImage.fillAmount = targetFill;
    }
}
