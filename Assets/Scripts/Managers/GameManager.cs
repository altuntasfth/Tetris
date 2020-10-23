using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Board m_gameBoard;
    private Spawner m_spawner;
    private Shape m_aciveShape;

    private float m_timeToDrop;
    private float m_dropInterval = 0.9f;
/*
    [Range(0.02f, 1.0f)] 
    public float m_keyRepeatRate = 0.25f;
    private float m_timeToNextKey;
*/    
    [Range(0.02f, 1.0f)] 
    public float m_keyRepeatRateLeftRight = 0.15f;
    private float m_timeToNextKeyLeftRight;
    
    [Range(0.01f, 1.0f)] 
    public float m_keyRepeatRateDown = 0.01f;
    private float m_timeToNextKeyDown;
    
    [Range(0.02f, 1.0f)] 
    public float m_keyRepeatRateRotate = 0.25f;
    private float m_timeToNextKeyRotate;

    void Start()
    {
        m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;
        m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;
        m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;

        //m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
        //m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

        m_gameBoard = GameObject.FindObjectOfType<Board>();
        m_spawner = GameObject.FindObjectOfType<Spawner>();

        if (!m_gameBoard)
        {
            Debug.LogWarning("WARNING: There is no game board defined!");
        }

        if (!m_spawner)
        {
            Debug.LogWarning("WARNING: There is no spawner defined!");
        }
        else
        {
            if (!m_aciveShape)
            {
                m_aciveShape = m_spawner.SpawnShape();
            }

            //Vector3Int.RoundToInt(m_spawner.transform.position);
            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);
        }
    }

    void Update()
    {
        if (!m_gameBoard || !m_spawner || !m_aciveShape)
        {
            return;
        }

        PlayerInput();
    }

    private void PlayerInput()
    {
        if (Input.GetButton("MoveRight") && (Time.time > m_timeToNextKeyLeftRight) || Input.GetButtonDown("MoveRight"))
        {
            m_aciveShape.MoveRight();
            m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;

            if (!m_gameBoard.IsValidPosition(m_aciveShape))
            {
                m_aciveShape.MoveLeft();
            }
        }
        else if (Input.GetButton("MoveLeft") && (Time.time > m_timeToNextKeyLeftRight) || Input.GetButtonDown("MoveLeft"))
        {
            m_aciveShape.MoveLeft();
            m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;

            if (!m_gameBoard.IsValidPosition(m_aciveShape))
            {
                m_aciveShape.MoveRight();
            }
        }
        else if (Input.GetButtonDown("Rotate") && (Time.time > m_timeToNextKeyRotate))
        {
            m_aciveShape.RotateRight();
            m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;

            if (!m_gameBoard.IsValidPosition(m_aciveShape))
            {
                m_aciveShape.RotateLeft();
            }
        }
        else if (Input.GetButton("MoveDown") && (Time.time > m_timeToNextKeyDown) || (Time.time > m_timeToDrop))
        {
            m_timeToDrop = Time.time + m_dropInterval;
            m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;

            m_aciveShape.MoveDown();

            if (!m_gameBoard.IsValidPosition(m_aciveShape))
            {
                LandShape();
            }
        }
    }

    private void LandShape()
    {
        m_timeToNextKeyLeftRight = Time.time;
        m_timeToNextKeyDown = Time.time;
        m_timeToNextKeyRotate = Time.time;
        
        m_aciveShape.MoveUp();
        m_gameBoard.StoreShapeInGrid(m_aciveShape);
        m_aciveShape = m_spawner.SpawnShape();
        
        m_gameBoard.ClearAllRows();
    }
}