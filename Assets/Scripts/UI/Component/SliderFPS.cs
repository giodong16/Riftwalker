using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SliderFPS : MonoBehaviour
{
    public Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(SettingFPS);
        LoadFPS();
    }

    private void SettingFPS(float value)
    {
        int targetFPS = 60;
        if (value == 0) targetFPS = 30;
        else if (value == 2) targetFPS = 120;

        Application.targetFrameRate = targetFPS;
        Pref.FPSLimit = targetFPS;
    }

    public void LoadFPS()
    {
        int currentFPS = Pref.FPSLimit;
        if (currentFPS == 30) slider.value = 0;
        else if (currentFPS == 60) slider.value = 1;
        else slider.value = 2;
        Application.targetFrameRate = currentFPS;
    }
}
