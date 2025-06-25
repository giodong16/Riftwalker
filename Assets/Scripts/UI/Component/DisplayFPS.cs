using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.UIElements;

public class DisplayFPS : MonoBehaviour
{
    public Text fpsText;
    private float deltaTime = 0.0f;
    private void Awake()
    {
        Application.targetFrameRate = Pref.FPSLimit;
    }
    void OnEnable()
    {
        StartCoroutine(UpdateFPS());
    }
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    IEnumerator UpdateFPS()
    {
        while (true)
        {
            float fps = 1.0f / deltaTime;
            fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();
            yield return new WaitForSeconds(1f);
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

}
