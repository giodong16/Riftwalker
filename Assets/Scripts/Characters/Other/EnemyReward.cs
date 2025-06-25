using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReward : MonoBehaviour
{
    public GameObject[] rewards;
    private Vector3[] startPositionRewards;
    public Transform parentTransform;
    public float durationTime = 1.5f;

    private void Start()
    {
        if (transform.parent != null)
        {
            parentTransform = transform.parent;
        }
        else
        {
            parentTransform = transform;
        }

        if (rewards == null || rewards.Length == 0) return;

        startPositionRewards = new Vector3[rewards.Length];

        for (int i = 0; i < rewards.Length; i++)
        {
            startPositionRewards[i] = rewards[i].transform.position;
            rewards[i].transform.position = parentTransform.position;
            rewards[i].SetActive(false);
        }
    }

    public void ShowRewards()
    {
        StartCoroutine(ShowRewardsEffect());
    }

    IEnumerator ShowRewardsEffect()
    {
        float elapsed = 0f;

        for (int i = 0; i < rewards.Length; i++)
        {
            if (rewards[i] != null)
            {
                rewards[i].SetActive(true);
                rewards[i].transform.position = parentTransform.position;
            }

        }

        while (elapsed < durationTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / durationTime;

            for (int i = 0; i < rewards.Length; i++)
            {
                Vector3 startPos = parentTransform.position;
                Vector3 targetPos = startPositionRewards[i];
                if (rewards[i] != null) {
                    rewards[i].transform.position = Vector3.Lerp(startPos, targetPos, t);
                }
                
            }

            yield return null;
        }

        for (int i = 0; i < rewards.Length; i++)
        {
            if (rewards[i] != null)
                rewards[i].transform.position = startPositionRewards[i];

        }
    }

}
