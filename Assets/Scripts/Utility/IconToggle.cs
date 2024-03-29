﻿using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class IconToggle : MonoBehaviour
{
    public Sprite m_iconTrue;
    public Sprite m_iconFalse;

    public bool m_defaultIconState = true;

    private Image m_image;

    private void Start()
    {
        m_image = GetComponent<Image>();
        m_image.sprite = (m_defaultIconState) ? m_iconTrue : m_iconFalse;
    }

    public void ToggleIcon(bool state)
    {
        if (!m_image || !m_iconTrue || !m_iconFalse)
        {
            Debug.LogWarning("WARNING! ICONTOGGLE missing iconTrue or iconFalse");
            return;
        }

        m_image.sprite = (state) ? m_iconTrue : m_iconFalse;
    }
}
