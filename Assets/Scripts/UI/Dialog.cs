using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public Text titleText;
    public Text messageText;

    public void UpdateUI(string title, string message)
    {
        titleText.text = title;
        messageText.text = message;
    }
}
