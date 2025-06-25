using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    public Image fadeImage;
    public float timeFullAlpha = 2f;
    public float fadeSpeed = 0.05f;
    public void FadeWhite() => StartCoroutine(Fade(Color.white));
    public void FadeBlack() => StartCoroutine(Fade(Color.black));

    IEnumerator Fade(Color color)
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(color.r, color.g, color.b, 0f);

        // Fade in
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            fadeImage.color = new Color(color.r, color.g, color.b, t);
            yield return fadeSpeed;
        }

        yield return new WaitForSeconds(timeFullAlpha); 

        // Fade out
        for (float t = 1; t > 0; t -= Time.deltaTime)
        {
            fadeImage.color = new Color(color.r, color.g, color.b, t);
            yield return fadeSpeed;
        }

        fadeImage.gameObject.SetActive(false);
    }
}
