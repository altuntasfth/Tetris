using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Board m_gameBoard;
    private Spawner m_spawner;
    private Shape m_aciveShape;

    private float m_timeToDrop;
    private float m_dropInterval = 0.25f;
    
    void Start()
    {
        //m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
        //m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

        m_gameBoard = GameObject.FindObjectOfType<Board>();
        m_spawner = GameObject.FindObjectOfType<Spawner>();

        if (m_spawner)
        {
            if (m_aciveShape == null)
            {
                m_aciveShape = m_spawner.SpawnShape();
            }
            //Vector3Int.RoundToInt(m_spawner.transform.position);
            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);
        }

        if (!m_gameBoard)
        {
            Debug.LogWarning("WARNING: There is no game board defined!");
        }
        
        if (!m_spawner)
        {
            Debug.LogWarning("WARNING: There is no spawner defined!");
        }
    }

    void Update()
    {
        if (!m_gameBoard || !m_spawner)
        {
            return;
        }

        if (Time.time > m_timeToDrop)
        {
            m_timeToDrop =  Time.time + m_dropInterval;
            if (m_aciveShape)
            {
                m_aciveShape.MoveDown();
                
                if (!m_gameBoard.IsValidPosition(m_aciveShape))
                {
                    m_aciveShape.MoveUp();
                    m_gameBoard.StoreShapeInGrid(m_aciveShape);
                    
                    if (m_spawner)
                    {
                        m_aciveShape = m_spawner.SpawnShape();
                    }
                }
            }
        }
    }
}
