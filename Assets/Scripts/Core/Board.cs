﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Transform m_emptySprite;
    public int m_height = 30;
    public int m_width = 10;
    public int m_header = 8;

    private Transform[,] m_grid;

    private void Awake()
    {
        m_grid = new Transform[m_width, m_height];
    }

    void Start()
    {
        DrawEmptyCells();
    }

    void Update()
    {
        
    }

    void DrawEmptyCells()
    {
        if (m_emptySprite != null)
        {  
            for (int y = 0; y < m_height - m_header; y++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    Transform clone;
                    clone = Instantiate(m_emptySprite, new Vector3(x,y,0), Quaternion.identity) as Transform;
                    clone.name = "Board Space ( x = " + x.ToString() + " , y = " + y.ToString() + " )";
                    clone.transform.parent = transform;
                } 
            }
        }
        else
        {
            Debug.LogWarning("WARNING: Please assign the emptySprite object!");
        }
    }
}