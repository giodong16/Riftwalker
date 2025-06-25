using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioMixer audioMixer;

    public Sound[] musicSounds;
    public AudioClip mouseClip;
    public AudioClip messageClip;
    public AudioClip warningClip;
    public AudioSource musicSource;
    public AudioSource sfxSource, sfxLoop;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        PlayMusic(NameSound.Music01.ToString());
        ToggleMusic();
        ToggleSFX();
    }
    public void PlayMusic(string name)
    {
        if (musicSounds == null || musicSource == null || musicSounds.Length <= 0) return;
        Sound s = Array.Find(musicSounds, a => a.name == name);

        if (s == null)
            return;
        else
        {
            musicSource.clip = s.clip;
            musicSource.loop = true;
            musicSource.Play();
        }

    }

    public void PlaySFX(AudioClip audioClip,float volume = 1f)
    {
        if (sfxSource == null || audioClip == null) return;
            sfxLoop.volume = volume;
            sfxSource.PlayOneShot(audioClip);     
    }
    public void PlaySFX(AudioSource audioSource, AudioClip audioClip,float volume = 1f)
    {
        if (audioSource == null || audioClip == null) return;
        audioSource.volume = volume;
        audioSource.PlayOneShot(audioClip);
    }
    public void PlaySFXLoop(AudioSource audioSource, AudioClip audioClip, float volume = 1f)
    {
        if (audioSource == null || audioClip == null) return;

        if (!audioSource.isPlaying)
        {
            audioSource.clip = audioClip;
            audioSource.loop = true;
            audioSource.volume = volume;
            audioSource.Play();
        }
    }
    public void PlaySFXLoop(AudioClip audioClip,float volume = 1f)
    {
        if (sfxLoop == null || audioClip == null) return;

        if (!sfxLoop.isPlaying)
        {
            sfxLoop.clip = audioClip;
            sfxLoop.loop = true;
            sfxLoop.volume= volume;
            sfxLoop.Play();
        }
    }

    public void PlayUIClick(float volume = 0.1f)
    {
        if (sfxSource == null || mouseClip == null) return;
        sfxSource.volume = volume;
        sfxSource.PlayOneShot(mouseClip);
    }
    public void PlayMessageSound(float volume = 1f)
    {
        if (sfxSource == null || messageClip == null) return;
        sfxSource.volume = volume;
        sfxSource.PlayOneShot(messageClip);
    }
    public void StopSFXLoop(AudioSource audioSource =null)
    {
        if (audioSource == null ) sfxLoop.Stop();
        else audioSource.Stop();
    }
    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }
    public void ToggleMusic()
    {
        PlayUIClick();
        if (audioMixer != null)
        {
            float volume = Pref.VolumeMusic;
            audioMixer.SetFloat("MusicVolume", volume <= 0.0001f ? -80f : Mathf.Log10(volume) * 20f);
        }
    }

    public void ToggleSFX()
    {
        PlayUIClick();
        if (audioMixer != null)
        {
            float volume = Pref.VolumeSFX;
            audioMixer.SetFloat("SFXVolume", volume <= 0.0001f ? -80f : Mathf.Log10(volume) * 20f);
        }
    }

}

public enum NameSound
{//player
    Jump,
    Land,
    Hurt,
    Death,
    Punch,
    DoublePunch,
    Throw,
    Bow,
    SlashSword,

    // âm thanh do destroy nhanh, thiếu hppotion
    CollectCoin,
    CollectKey,
    WoodBreak,
    //UI
    UIClick,
    Win,
    Gameover,
    Message,
    RequireDialog,
    //Theme
    Music01,

}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}
