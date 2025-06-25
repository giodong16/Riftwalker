using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageCanvas : MonoBehaviour
{
    public Text titleText;
    public Text messageText;

    public GameObject messagePanel;
    public GameObject quitMessage;
    public void UpdateUI(string title, string message, bool isQuit = false)
    {
        ShowQuitMessage(isQuit);
        if (!isQuit)
        {
            titleText.text = title;
            messageText.text = message;
        }
       
    }
    public void ShowQuitMessage(bool isQuit)
    {
        
        quitMessage.SetActive(isQuit);
        messagePanel.SetActive(!isQuit);
        AudioManager.Instance.PlayMessageSound(1);
    }
}
