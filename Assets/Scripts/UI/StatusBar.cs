using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [Header("Heart Bar")]
    public Text heartText;

    [Header("Bullet Bar")]
    public Text bulletText;
    public ItemData bulletData;
    public Animator animatorBulletText;

    [Header("Coins Bar")]
    public Text coinsText;

    [Header("Souls Bar")]
    public Text soulsText;
    public Animator animatorSoulText;

    private void OnEnable()
    {
        SetUp();
    }
    public void SetUp()
    {
        UpdateBulletText();
        UpdateCoinsText();
        UpdateHeartText();
        UpdateSoulsText();
    }

    public void UpdateHeartText()
    {
        heartText.text = "x" + GameController.Instance?.currentHeart.ToString();
    }

    public void UpdateCoinsText()
    {
        coinsText.text = "x" + GameController.Instance?.CoinPerLevel.ToString();
    }

    public void UpdateSoulsText() { 
        soulsText.text = "x" + GameController.Instance?.currentSouls.ToString()+"/" +GameController.Instance?.currentLevelData.soulRequired;
    }

    public void UpdateBulletText()
    {
        bulletText.text = "x" + ItemDataManager.Instance.GetQuality(bulletData).ToString();
    }

    public void WarningSouls()
    {
        AudioManager.Instance?.PlaySFX(AudioManager.Instance.warningClip);
        if (animatorSoulText != null) {
            animatorSoulText.SetTrigger("warning");
        }

    }
    public void WarningBullets()
    {
        if (animatorBulletText != null)
        {
            animatorBulletText.SetTrigger("warning");
        }
    }
}
