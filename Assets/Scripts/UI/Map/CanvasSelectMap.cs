using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSelectMap : MonoBehaviour
{
    public GameObject[] selectLevelPanels;
    public GameObject selectLevelRoot;
    public GameObject selectMapRoot;

    private void OnEnable()
    {
        ShowMapRoot(true);
    }
    private void OnDisable()
    {
        selectMapRoot.SetActive(false);
        selectLevelRoot.SetActive(false);
    }
    public void ShowSelectLevelPanel(int index)
    {
        ShowMapRoot(false);
        HideAllPanel();
        selectLevelPanels[index].SetActive(true);
    }
    public void HideAllPanel()
    {
        for (int i = 0; i < selectLevelPanels.Length; i++)
        {
            selectLevelPanels[i].SetActive(false);
        }
    }
    public void CloseSelectLevel()
    {
        ShowMapRoot(true);
    }
    public void ShowMapRoot(bool isShow)
    {
        selectLevelRoot.SetActive(!isShow);
        selectMapRoot.SetActive(isShow);
    }



   
}
