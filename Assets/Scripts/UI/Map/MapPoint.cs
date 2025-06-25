using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPoint : MonoBehaviour
{
    public Button levelButton;
    public Image[] starImages;
    private void Awake()
    {
        levelButton = GetComponent<Button>();
    }
    public void SetUp(bool isUnlock, int stars)
    {
        SetUpStar(isUnlock, stars);
        SetUpButton(isUnlock);
    }
    public void SetUpStar(bool isUnlock, int stars)
    {
        for(int i = 0;i<starImages.Length;i++)
        {
            starImages[i].enabled = isUnlock;
            if(i+1 <= stars)
            {
                starImages[i].color = Color.yellow;
            }
            else
            {
                starImages[i].color = Color.white;
            }
        }
    }
    public void SetUpButton(bool isUnlock)
    {
        levelButton.interactable = isUnlock;
    }
}
