﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public AudioClip m_gameOverSound;
    public AudioClip m_backgroundSound;
    
}
