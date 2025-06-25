using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPortal : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    ParticleSystem particle;
    private StatusBar statusBar;
    public float timeDelay = 0.5f;
    bool isWon = false;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (/*!isWarning && */collision.CompareTag("Player") && !isWon)
        {
            
            if (!GameController.Instance.IsCompletedSoulsRequire())
            {
                if (statusBar == null)
                {
                    statusBar = FindObjectOfType<StatusBar>();
                }
                if (statusBar != null)
                {
                    statusBar.WarningSouls();
                    
                }
            }
            else
            {
                isWon = true;
                StartCoroutine(DelayWin());
            }
        }
    }
   
    IEnumerator DelayWin()
    {
        yield return new WaitForSecondsRealtime(timeDelay);
        particle.Stop();
        GameController.Instance.Win();
    }
}
