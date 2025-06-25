using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour
{
    public Text timerText;

    private int elsapsedTime = 0;
    private bool isRunning = false;
    private Coroutine timerCoroutine;


    public void UpdateTimerUI()
    {
        timerText.text = FormatTime(elsapsedTime);
    }
    private string FormatTime(int totalSeconds) 
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public int GetTime()
    {
        return elsapsedTime;
    }

    IEnumerator Timer()
    {
        while (isRunning)
        {
            UpdateTimerUI();
            yield return new WaitForSeconds(1);
            elsapsedTime++;
        }
    }
    public void StartTimer()
    {
        elsapsedTime = 0;
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        isRunning = true;
        timerCoroutine = StartCoroutine(Timer());
    }
    public void StopTimer()
    {
        isRunning = false;
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
    }
}
