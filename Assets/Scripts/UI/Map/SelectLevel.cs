using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevel : MonoBehaviour
{
    public List<GameObject> listMapPoints;
    public MapData mapData;

    private void OnEnable()
    {
        LoadMapPoints();
    }
    private void OnDisable()
    {
        ClearAllEvents();
    }
    public void LoadMapPoints() 
    {
        if(listMapPoints == null || mapData == null || mapData.levels.Count > listMapPoints.Count) return;
        int unlockLevel = Pref.UnlockLevel;
        int levels = mapData.levels.Count;
        for(int i = 0; i < levels; i++)
        {
            int levelNumber = mapData.levels[i].levelNumber;
            bool isUnlock = unlockLevel >=levelNumber;
            int stars = Pref.GetStarsForLevel(levelNumber);
            listMapPoints[i].SetActive(true);
            listMapPoints[i].GetComponent<MapPoint>().SetUp(isUnlock, stars);
            listMapPoints[i].GetComponent<MapPoint>().levelButton.onClick.AddListener(() => GameController.Instance.LoadLevel(levelNumber));
        }
        if (levels < listMapPoints.Count) {
            for (int i = levels; i < listMapPoints.Count; i++) {
                listMapPoints[i].SetActive(false);
            }
        }
    }
    public void ClearAllEvents()
    {
        for (int i = 0; i < mapData.levels.Count; i++) 
        { 
            listMapPoints[i].GetComponent<MapPoint>().levelButton.onClick.RemoveAllListeners();
        }
    }
}
