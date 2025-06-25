using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    [Header("Coins")]
    public Text coinsText;
    public Button buyCoinsButton;
    private void Awake()
    {
        UpdateCoinsUI();
        buyCoinsButton.onClick.AddListener(Message);
    }
    private void OnEnable()
    {
        UpdateCoinsUI();
    }
    public void UpdateCoinsUI()
    {
        coinsText.text = Pref.Coins.ToString();
    }
    public void Message()
    {
        GameController.Instance.OnShowMessage("Message", Pref.MESSAGE_UNVAIABLE.ToString());
    }

}
