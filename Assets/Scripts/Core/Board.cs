﻿using System.Collections;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Transform m_emptySprite;
    public int m_height = 30;
    public int m_width = 10;
    public int m_header = 8;
    public int m_completedRows = 0;

    public ParticlePlayer[] m_rowGlowFx = new ParticlePlayer[4];

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

    bool IsWithinBoard(int x, int y)
    {
        return (x >= 0 && x < m_width && y >= 0);
    }

    bool IsOccupied(int x, int y, Shape shape)
    {
        return (m_grid[x, y] != null && m_grid[x, y].parent != shape.transform);
    }

    public bool IsValidPosition(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vectorf.Round(child.position);

            if (!IsWithinBoard((int) pos.x, (int) pos.y))
            {
                return false;
            }

            if (IsOccupied((int) pos.x, (int) pos.y, shape))
            {
                return false;
            }
        }

        return true;
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
                    clone = Instantiate(m_emptySprite, new Vector3(x, y, 0), Quaternion.identity) as Transform;
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

    public void StoreShapeInGrid(Shape shape)
    {
        if (shape == null)
        {
            return;
        }

        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vectorf.Round(child.position);
            m_grid[(int) pos.x, (int) pos.y] = child;
        }
    }

    bool IsComplete(int y)
    {
        for (int x = 0; x < m_width; ++x)
        {
            if (m_grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    private void ClearRow(int y)
    {
        for (int x = 0; x < m_width; ++x)
        {
            if (m_grid[x, y] != null)
            {
                Destroy(m_grid[x, y].gameObject);
            }

            m_grid[x, y] = null;
        }
    }

    private void ShiftOneRowDown(int y)
    {
        for (int x = 0; x < m_width; ++x)
        {
            if (m_grid[x, y] != null)
            {
                m_grid[x, y - 1] = m_grid[x, y];
                m_grid[x, y] = null;
                m_grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    private void ShiftRowsDown(int startY)
    {
        for (int i = startY; i < m_height; ++i)
        {
            ShiftOneRowDown(i);
        }
    }

    public IEnumerator ClearAllRows()
    {
        m_completedRows = 0;
        
        for (int y = 0; y < m_height; ++y)
        {
            if (IsComplete(y))
            {
                ClearRowFX(m_completedRows, y);
                m_completedRows++;
            }
        }

        yield return new WaitForSeconds(0.5f);
        
        for (int y = 0; y < m_height; ++y)
        {
            if (IsComplete(y))
            {
                ClearRow(y);
                ShiftRowsDown(y + 1);
                yield return new WaitForSeconds(0.3f);
                y--;
            }
        }
    }

    public bool IsOverLimit(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            if (shape.transform.position.y >= (m_height - m_header - 1))
            {
                return true;
            }
        }

        return false;
    }

    private void ClearRowFX(int idx, int y)
    {
        if (m_rowGlowFx[idx])
        {
            m_rowGlowFx[idx].transform.position = new Vector3(0, y, -2f);
            m_rowGlowFx[idx].Play();
        }
    }
}