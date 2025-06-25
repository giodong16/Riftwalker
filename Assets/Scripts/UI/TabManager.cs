using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabManager : MonoBehaviour
{
    public GameObject[] panels;
    public GameObject[] borderTabTitles;
    private ShopManager shopManager;
    private void Awake()
    {
        shopManager = FindObjectOfType<ShopManager>();
        panels[0].SetActive(true);
        borderTabTitles[0].SetActive(true);
    }

    public void ShowTab(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
            borderTabTitles[i].SetActive(false);
        }
        panels[index].SetActive(true);
        shopManager.RefreshUI();
        borderTabTitles[index].SetActive(true);
    }
}

   