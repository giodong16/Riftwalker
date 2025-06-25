using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressMap : MonoBehaviour
{
    public MapData mapData;
    public Slider slider;
    public GameObject dot;
    public Transform root;
    private void Awake()
    {
        ClearProgress();
    }
    private void OnEnable()
    {
        SetUpProgessBar();
    }
    private void OnDisable()
    {
        ClearProgress();
    }
    public void SetUpProgessBar()
    {
        if (mapData == null||mapData.levels == null|| mapData.levels.Count <= 0) return;

        int unlockLevel = Pref.UnlockLevel;
        int count = mapData.levels.Count;
        slider.maxValue = count - 1;

        for (int i = 0; i < count; i++)
        {
            GameObject dotComponent = Instantiate(dot, root);
            dotComponent.SetActive(true);

            if (mapData.levels[i] != null)
            {
                var dotInProgress = dotComponent.GetComponent<DotInProgessLevel>();
                bool isUnlocked = mapData.levels[i].levelNumber <= unlockLevel;

                dotInProgress.SetUp(isUnlocked);

                if (mapData.levels[i].levelNumber == Pref.CurrentLevelPlay)
                {
                    dotInProgress.TurnOnLight(true);
                }
            }
        }
        int countLevelUnlockInMap = unlockLevel - mapData.levels[0].levelNumber + 1;
        slider.value = Mathf.Clamp(countLevelUnlockInMap-1, 0, count - 1);
    }

    public void ClearProgress()
    {
        foreach (Transform child in root)
        {
            Destroy(child.gameObject);
        }
    }
}
