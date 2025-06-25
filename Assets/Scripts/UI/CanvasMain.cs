using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasMain : MonoBehaviour
{
    private static CanvasMain Instance;
    private Canvas canvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            canvas = GetComponent<Canvas>();
            SetCameraToMain();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetCameraToMain();
    }

    private void SetCameraToMain()
    {
        Camera mainCam = Camera.main;
        if (mainCam != null && canvas != null)
        {
            canvas.worldCamera = mainCam;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
