using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Board m_gameBoard;
    private Spawner m_spawner;
    
    void Start()
    {
        //m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
        //m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

        m_gameBoard = GameObject.FindObjectOfType<Board>();
        m_spawner = GameObject.FindObjectOfType<Spawner>();

        if (m_spawner)
        {                                //Vector3Int.RoundToInt(m_spawner.transform.position);
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
        
    }
}
