using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIEffect : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 0.3f;

    private void Awake()
    {
        if(canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }
    public void SetCanvasGroup(bool isShow)
    {
        canvasGroup.alpha = isShow ? 1 : 0;
        canvasGroup.interactable = isShow;
        canvasGroup.blocksRaycasts = isShow;
    }
    public void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }

    public void Hide() {
        // bật coroutine
        if(gameObject.activeSelf) 
            StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float timeCount = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        while (timeCount < fadeDuration) {
            timeCount += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(1,0,timeCount/fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }

    private IEnumerator FadeIn()
    {
        
        float timeCount = 0;
        
        while (timeCount < fadeDuration)
        {
            timeCount += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, timeCount / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

}
