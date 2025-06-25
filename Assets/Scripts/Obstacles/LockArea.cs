using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockArea : MonoBehaviour
{
    public Transform targetPosBottom;
    public Transform targetPosTop;

    public Transform startPosBottom;
    public Transform startPosTop;

    public float durationTime;

    public GameObject topObstacle;
    public GameObject bottomObstacle;

    public List<FollowEnemy> enemies;
    int countEnemies;
    private void Start()
    {
        startPosBottom = bottomObstacle.transform;
        startPosTop = topObstacle.transform;
        Lock();

        // Gán tham chiếu LockArea cho từng quái
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.SetUp(this);
            }
        }
        countEnemies = enemies.Count;
    }

    public void DestroyEnemy()
    {
        countEnemies = countEnemies - 1;
        if(countEnemies > 0)  return;

        Open(); 
    }

    public void Lock()
    {
        if (topObstacle != null && startPosTop != null)
        {
            StartCoroutine(MoveToTarget(topObstacle, startPosTop.position));
        }
        if (bottomObstacle != null && startPosBottom != null)
        {
            StartCoroutine(MoveToTarget(bottomObstacle, startPosBottom.position));
        }
    }

    public void Open()
    {
        if (topObstacle != null && targetPosTop!=null) {
            StartCoroutine(MoveToTarget(topObstacle,targetPosTop.position) );
        }
        if (bottomObstacle != null && targetPosBottom != null)
        {
            StartCoroutine(MoveToTarget(bottomObstacle, targetPosBottom.position));
        }
    }

    IEnumerator MoveToTarget(GameObject gameObject, Vector2 targetPos)
    {
        float elapsedTime = 0;
        Vector2 startPos = gameObject.transform.position;
        while (elapsedTime < durationTime) { 
            elapsedTime += Time.deltaTime;
            float t= elapsedTime / durationTime;
            gameObject.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
        gameObject.transform.position = targetPos;
    }

}
