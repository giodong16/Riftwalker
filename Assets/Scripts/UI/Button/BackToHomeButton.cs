using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackToHomeButton : MonoBehaviour
{
    Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
      
    }
    private void Start()
    {
        button.onClick.AddListener(GameController.Instance.OnBackToHomeButtonClicked);
    }
}
