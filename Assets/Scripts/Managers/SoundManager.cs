using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public bool m_musicEnabled = true;
    public bool m_fxEnabled = true;

    [Range(0,1)]
    public float m_musicVolume = 1.0f;
    
    [Range(0,1)]
    public float m_fxVolume = 1.0f;

    public AudioClip m_clearRowSound;
    public AudioClip m_moveSound;
    public AudioClip m_dropSound;
    public AudioClip m_errorSound;
    public AudioClip m_gameOverSound;
    //public AudioClip m_backgroundSound;
    
    public AudioSource m_musicSource;

    public AudioClip[] m_musicClips;
    private AudioClip m_randomMusicClip;

    private void Start()
    {
        //m_randomMusicClip = GetRandomClip(m_musicClips);
        //PlayBackgroundMusic(m_randomMusicClip);
        
        PlayBackgroundMusic(GetRandomClip(m_musicClips));
    }

    private AudioClip GetRandomClip(AudioClip[] clips)
    {
        AudioClip randomClip = clips[Random.Range(0, clips.Length)];
        return randomClip;
    }

    private void PlayBackgroundMusic(AudioClip musicClip)
    {
        if (!m_musicEnabled || !musicClip || !m_musicSource)
        {
            return;
        }
        
        m_musicSource.Stop();
        m_musicSource.clip = musicClip;
        m_musicSource.volume = m_musicVolume;
        m_musicSource.loop = true;
        m_musicSource.Play();
    }

    private void UpdateMusic()
    {
        if (m_musicSource.isPlaying != m_musicEnabled)
        {
            if (m_musicEnabled)
            {
                PlayBackgroundMusic(GetRandomClip(m_musicClips));
            }
            else
            {
                m_musicSource.Stop();
            }
        }
    }

    public void ToggleMusic()
    {
        m_musicEnabled = !m_musicEnabled;
        UpdateMusic();
    }

    public void ToggleFX()
    {
        m_fxEnabled = !m_fxEnabled;
    }
}
