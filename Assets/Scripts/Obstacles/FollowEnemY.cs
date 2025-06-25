using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : MonoBehaviour
{
    public LockArea lockArea;
    public void SetUp(LockArea lockArea)
    {
        this.lockArea = lockArea;
    }
    private void OnDestroy()
    {
        if (lockArea != null) { 
            lockArea.DestroyEnemy();
        }
    }
}
