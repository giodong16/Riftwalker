using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHeathBarEnemy : MonoBehaviour
{
    public Slider slider;
    public void UpdateHeathBar(float current, float max)
    {
        if (slider == null) return;
        slider.value = current / max;
    }
}
