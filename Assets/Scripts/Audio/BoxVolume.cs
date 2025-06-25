using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoxVolume : MonoBehaviour
{
    public AudioSource[] audioSources;
    bool isInBox = false;
    private void Awake()
    {// nên đặt các audioSource là playonawake true tránh lỗi đập nhau;
        TurnOffOrOnVolume();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isInBox && collision.CompareTag("Player"))
        {
            isInBox = true;
            TurnOffOrOnVolume();
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isInBox && collision.CompareTag("Player"))
        {
            isInBox = false;
            TurnOffOrOnVolume();
        }
    }
    private void TurnOffOrOnVolume()
    {
        if (audioSources == null || audioSources.Length <= 0) return;
        float volume = isInBox ? Pref.VolumeSFX : 0.0f;
        foreach (AudioSource source in audioSources)
        {
            if (source != null)
            {
                source.volume = volume;  
            }
        }
    }
}
