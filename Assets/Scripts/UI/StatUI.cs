using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    public Image icon;
    public Text text;
    public void SetUp(Sprite sprite, string content)
    {
        icon.sprite = sprite;
        text.text = " +"+content;
    }
}
