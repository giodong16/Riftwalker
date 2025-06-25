using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum GameState
{
    Start,
    Playing,
    Pause,
    Gameover,
    Win,

}
public class GameController : MonoBehaviour
{
    public static GameController Instance;
    private UIManager uiManager;
    private bool isLoadingScene = false;
    
    public GameState gameState;
    public AudioClip winSound;
    public AudioClip loseSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        uiManager = UIManager.Instance;
    }
    


    #region Status Game
    // Status game
    int coinPerLevel = 0;
    int time;
    public int CoinPerLevel { get => coinPerLevel;private set => coinPerLevel = value; }

    public void Win()
    {
        AudioManager.Instance?.PlaySFX(winSound);

        gameState = GameState.Win;
        uiManager.ShowUI(CanvasName.Canvas_WinDialog);
        Summary();
        Pref.UnlockLevel = currentLevel+1;
        Time.timeScale = 0;
    }
    public void Lose()
    {
        AudioManager.Instance?.PlaySFX(loseSound);
        gameState = GameState.Gameover;
        ResetData();  
        uiManager.ShowUI(CanvasName.Canvas_LoseDialog); 
        Time.timeScale = 0;
       // LevelTimer levelTimer = FindObjectOfType<LevelTimer>();
    }

    public void LoadScene(string sceneName)
    {
        if(!IsSceneInBuild(sceneName)) return;
        
        uiManager.ShowUI(CanvasName.Canvas_Transition);
        FindObjectOfType<SceneController>().LoadSceneWithTransition(sceneName); 
    }

/*    public bool IsCanWin()
    {
        return currentSouls >= currentLevelData.soulRequired;
    }*/
    public bool IsCompletedHeartRequire()
    {
        return currentHeart >= currentLevelData.livesLeftRequire;
    }
    public bool IsCompletedSoulsRequire()
    {
        return currentSouls >= currentLevelData.soulRequired;
    }
    public bool IsCompletedTimeRequire()
    {
        return time <= currentLevelData.timeRequire;
    }
    public int LevelRating() // đánh giá nhiệm vụ tính số sao đạt được
    {
        int ratingHeart = IsCompletedHeartRequire() ? 1 : 0;
        int ratingSoul = IsCompletedSoulsRequire() ? 1 : 0;
        int ratingTimer = IsCompletedTimeRequire() ? 1 : 0;   
        return ratingTimer + ratingHeart + ratingSoul;
    }
    public void Summary()
    {
        Pref.Coins += coinPerLevel;
        CanvasWinDialog canvasWinDialog = FindObjectOfType<CanvasWinDialog>();
        
        LevelTimer levelTimer = FindObjectOfType<LevelTimer>();
        if (levelTimer != null)
        {
            time = levelTimer.GetTime();
        }
        int stars = LevelRating();
        Pref.SaveStarsForLevel(currentLevel, stars);
        if (canvasWinDialog != null) {
            canvasWinDialog.Summary(coinPerLevel,currentSouls,time,stars); 

            canvasWinDialog.UpdateTask(IsCompletedHeartRequire(),IsCompletedSoulsRequire(),IsCompletedTimeRequire()
                ,currentLevelData.livesLeftRequire,currentLevelData.soulRequired,currentLevelData.timeRequire);
        }

        

        ResetData();
    }
    public void ResetData()
    {
        coinPerLevel = 0;
        currentSouls = 0;
        // nhớ reset time
    }

    #endregion


    #region Event button
    public void OnPauseButtonClicked()
    {
        gameState = GameState.Pause;
        uiManager.ShowUI(CanvasName.Canvas_PauseDialog);
        float time = uiManager.GetFadeTime(CanvasName.Canvas_PauseDialog);
        // StartCoroutine(DelayPause(time, 0));
        Time.timeScale = 0;
    }
    public void OnResumeButtonClicked() {
        gameState = GameState.Playing;
        uiManager.HideUI(CanvasName.Canvas_PauseDialog);
        Time.timeScale = 1;
    }

    public void OnSettingsButtonClicked()
    {
        uiManager.ShowUI(CanvasName.Canvas_Settings);
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("Home"))
        {
            Time.timeScale = 1;
        }
        else
        {
            float time = uiManager.GetFadeTime(CanvasName.Canvas_Settings);
            // StartCoroutine(DelayPause(time, 0));
            Time.timeScale = 0;
        }

    }

    public void OnCloseSettingsButtonClicked()
    {
        uiManager.HideUI(CanvasName.Canvas_Settings);
        Time.timeScale = 1;
    }

    public void OnInventoryButtonClicked()
    {
        uiManager.ShowUI(CanvasName.Canvas_Inventory);
    }

    public void OnCloseInventoryButtonClicked()
    {
        uiManager.HideUI(CanvasName.Canvas_Inventory);
    }

    public void OnReplayButtonClicked()
    {
        LoadLevel(currentLevel);
    }
    public void OnNextButtonClicked()
    {
        
        LoadLevel(currentLevel+1); // phải kiểm tra xem còn level nào không

    }
    public void OnBackToHomeButtonClicked()
    {
        gameState = GameState.Start;
        LoadLevel(0);
    }
    public void OnQuitButtonClicked()
    {
        #if UNITY_EDITOR
                EditorApplication.isPlaying = false;
        #else
        
                Application.Quit();
        #endif
    }
    public void OnBackToMapClicked()
    {
      //  LoadLevel(0);
        Time.timeScale = 0;
        OnOpenSelectMap();
    }
    public void OnShopButtonClicked()
    {
        uiManager.ShowUI(CanvasName.Canvas_Shop);
    }
    public void OnCloseShopButtonClicked()
    {
        uiManager.HideUI(CanvasName.Canvas_Shop);
    }

    public void OnOpenSelectMap()
    {
        uiManager.ShowUI(CanvasName.Canvas_SelectMap);
    }
    public void OnCloseSelectMap()
    {
        if (isLoadingScene) return;
        uiManager.HideUI(CanvasName.Canvas_SelectMap);
    }
    public void OnShowMessage(string title, string content)
    {
        uiManager.ShowUI(CanvasName.Canvas_MessageDialog);
        uiManager.messageCanvas.UpdateUI(title,content);
    }
    public void OnShowQuitMessage()
    {
        uiManager.ShowUI(CanvasName.Canvas_MessageDialog);
        uiManager.messageCanvas.ShowQuitMessage(true);
    }
    public void CloseMessage()
    {
        uiManager.HideUI(CanvasName.Canvas_MessageDialog);
    }
    #endregion

    #region Time
    IEnumerator DelayPause(float time, int timeScale)
    {
        yield return null;
        Time.timeScale = timeScale;
    }
    #endregion


    #region Level Manager
    public LevelData currentLevelData;
    public int currentLevel;
    public int currentSouls = 0;
    public int currentHeart;
    // thêm đếm time
    public string missionDescription;
   
    public void LoadLevel(int level)
    {
        if (isLoadingScene) return; 
        isLoadingScene = true;

        string formatted = level.ToString("D2");
        currentLevelData = Resources.Load<LevelData>($"Levels/Level_{formatted}");

        if (currentLevelData != null)
        {
            InitLevel();
            string nameScene = "Level_" + formatted;
            LoadScene(nameScene);
            gameState = GameState.Playing;
        }
        else
        {
            gameState = GameState.Start;
            LoadScene("Home");
        }
    }

    public void ResetSceneLoadFlag()
    {
        isLoadingScene = false;
    }
    void InitLevel() {
        Pref.CurrentLevelPlay = currentLevel;
        currentLevel = currentLevelData.levelNumber;
        currentHeart = currentLevelData.playerLives;
        missionDescription = currentLevelData.missionDescription;
        
        // cập nhật giao diện
    }


  

    public static bool IsSceneInBuild(string sceneName)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (name == sceneName)
                return true;
        }
        return false;
    }

    [ContextMenu("Unlock All Level")]
    public void UnlockAllLevel()
    {
        Pref.UnlockLevel = 10;
    }
    [ContextMenu("Hack Coins")]
    public void HackCoins()
    {
        Pref.Coins = 100000;
    }
    #endregion

    #region Collect
    public void CollectCoins(int coins)
    {
        coinPerLevel += coins;
        //Cập nhật giao diện
        StatusBar statusBar = FindObjectOfType<StatusBar>();
        if(statusBar != null)
        {
            statusBar.UpdateCoinsText();
        }
    }

    public void CollectSouls(int souls)
    {
        currentSouls += souls;
        StatusBar statusBar = FindObjectOfType<StatusBar>();
        if (statusBar != null)
        {
            statusBar.UpdateSoulsText();
        }
    }


    #endregion

}
