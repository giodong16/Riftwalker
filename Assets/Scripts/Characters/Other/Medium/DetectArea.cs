using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectArea : MonoBehaviour
{
    public MediumEnemy mediumEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (mediumEnemy)
            {
                mediumEnemy.detectedPlayer = true;
                mediumEnemy.player = collision.gameObject.transform;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (mediumEnemy)
            {
                mediumEnemy.detectedPlayer = false;
                mediumEnemy.player = null;
            }
        }
    }
}
