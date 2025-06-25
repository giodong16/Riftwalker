using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
     //   button.onClick.AddListener(GameController.Instance.OnPlayButtonClicked);
    }
}
