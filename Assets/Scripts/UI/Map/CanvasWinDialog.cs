using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasWinDialog : MonoBehaviour
{
    public Image[] imageStars;

    public Text coinsText;
    public Text soulsText;
    public Text timeText;

    [Header("Tasks")]
    public Task taskLive, taskSouls, taskTime; 

    public void Summary(int coins, int souls, int time,int stars)
    {
        UpdateCoinsText(coins);
        UpdateSoulsText(souls);
        UpdateTimeText(time);
        UpdateStars(stars);
    }
    public void UpdateCoinsText(int coins)
    {
        coinsText.text = coins.ToString();
    }

    public void UpdateSoulsText(int souls)
    {
        soulsText.text = souls.ToString();
    }

    public void UpdateTimeText(int time) {
        timeText.text =FormatTime(time);
    }
    private string FormatTime(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void UpdateStars(int stars)
    {
        for (int i = 0; i < imageStars.Length; i++) {
            if (i < stars)
            {
                imageStars[i].color = new Color(1, 1, 1, 1f);
            }
            else
            {
                imageStars[i].color = new Color(1, 1, 1, 0.2f);
            }
        }
    }

    public void UpdateTask(bool livesPassed, bool soulsPassed, bool timePassed, int livesRequire, int soulsRequire, int timeRequire)
    {
        taskLive.tickImage.enabled = livesPassed;
        taskSouls.tickImage.enabled = soulsPassed;
        taskTime.tickImage.enabled = timePassed;

        taskLive.DescriptionText.text = $"Have at least {livesRequire} life left";
        taskSouls.DescriptionText.text = $"Collect {soulsRequire}+ souls";
        taskTime.DescriptionText.text = $"Finish under {timeRequire}s";
    }
}
[System.Serializable]
public class Task
{
    public Image tickImage;
    public Text DescriptionText;
}
