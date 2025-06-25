using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlusText : MonoBehaviour
{
    public Text text;
    public float moveUpDistance = 0.5f;

    public void ShowPlusText(int quality, float duration)
    { 
        text.text = "+ " + quality;
        text.color = new Color(1, 1, 1, 1);
        StartCoroutine(TextEffect(duration));
    }
    private IEnumerator TextEffect(float duration = 0.5f)
    {
        float elapsedTime = 0f;
        
        Vector3 startPos = text.transform.position;
        Vector3 targetPos = startPos + Vector3.up *moveUpDistance;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            text.transform.position = Vector3.Lerp(startPos, targetPos, t);
            text.color = new Color(1, 1, 1, 1 - t); 
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(text.gameObject);
    }

}
