using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pref
{
    public static float VolumeMusic
    {
        set => PlayerPrefs.SetFloat(GameConsts.VOLUMEMUSIC.ToString(), value);
        get => PlayerPrefs.GetFloat(GameConsts.VOLUMEMUSIC.ToString(),1.0f);
    }
    public static float VolumeSFX
    {
        set => PlayerPrefs.SetFloat(GameConsts.VOLUMESFX.ToString(), value);
        get => PlayerPrefs.GetFloat(GameConsts.VOLUMESFX.ToString(), 1.0f);
    }
    public static int FPSLimit
    {
        set => PlayerPrefs.SetInt(GameConsts.FPSLimit.ToString(), value);
        get => PlayerPrefs.GetInt(GameConsts.FPSLimit.ToString(), 60);
    }


    // tạm thời
    public static int Coins
    {
        set => PlayerPrefs.SetInt(GameConsts.Coins.ToString(), value);
        get => PlayerPrefs.GetInt(GameConsts.Coins.ToString(),1000);
    }

    //Level and stars
    public static int UnlockLevel
    {
        set
        {
            int oldUnlockLevel = PlayerPrefs.GetInt(ProgessLevel.UNLOCKEDLEVEL.ToString());
            if (oldUnlockLevel < value)
            {
                PlayerPrefs.SetInt(ProgessLevel.UNLOCKEDLEVEL.ToString(), value);
            }
        }

        get => PlayerPrefs.GetInt(ProgessLevel.UNLOCKEDLEVEL.ToString(),1);
    }

    public static int CurrentLevelPlay
    {
        set
        {
            PlayerPrefs.SetInt(ProgessLevel.CURRENTLEVEL.ToString(), value);
        }

        get => PlayerPrefs.GetInt(ProgessLevel.CURRENTLEVEL.ToString(), 1);
    }
    // stars
    public static void SaveStarsForLevel(int level, int stars = 0)
    {
        string key = "Level_" + level + "_Stars"; // Level_1_Stars
        int old = PlayerPrefs.GetInt(key);
        if (old < stars)
            PlayerPrefs.SetInt(key, stars);
    }

    public static int GetStarsForLevel(int level)
    {
        string key = "Level_" + level + "_Stars";
        return PlayerPrefs.GetInt(key, 0);
    }
    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey(ProgessLevel.UNLOCKEDLEVEL.ToString());
        PlayerPrefs.DeleteKey(ProgessLevel.CURRENTLEVEL.ToString());
        for (int i = 1; i <= 10; i++) // TẠM ĐẶT LÀ 10
        {
            PlayerPrefs.DeleteKey($"Level_{i}_Stars");
        }
        PlayerPrefs.Save();
    }

    /*    //tính tổng số sao từ level [A,B) ví dụ 1-5 thì tính 1 đến hết 4
        public static int GetTotalStars(int A, int B)
        {
            int total = 0;
            for (int i = A; i < B; i++)
            {
                total += PlayerPrefs.GetInt("Level_" + i + "_Stars", 0);
            }
            return total;
        }*/

    public static int BasicEnemyDamage = 200;
    public static int MediumEnemyDamage = 500;
    public static int HighEnemyDamage = 800;
    public static string MESSAGE_COINS = "You need more gold to proceed";
    public static string MESSAGE_UNVAIABLE = "This feature is not available at this time.";
}
