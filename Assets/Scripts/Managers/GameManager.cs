﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Board m_gameBoard;
    private Spawner m_spawner;
    private Shape m_aciveShape;
    private SoundManager m_soundManager;
    private ScoreManager m_scoreManager;
    private Ghost m_ghost;
    private Holder m_holder;

    private float m_timeToDrop;
    public float m_dropInterval = 0.9f;

    private float m_dropIntervalModded;
   
    [Range(0.02f, 1.0f)] 
    public float m_keyRepeatRateLeftRight = 0.15f;
    private float m_timeToNextKeyLeftRight;
    
    [Range(0.01f, 1.0f)] 
    public float m_keyRepeatRateDown = 0.01f;
    private float m_timeToNextKeyDown;
    
    [Range(0.02f, 1.0f)] 
    public float m_keyRepeatRateRotate = 0.25f;
    private float m_timeToNextKeyRotate;

    private bool m_gameOver = false;

    public GameObject m_gameOverPanel;
    public GameObject m_pausePanel;

    public bool m_isPaused = false;

    public IconToggle m_rotIconTogle;

    public ParticlePlayer m_gameOverFx;
    
    private bool m_clockwise = true;
    
    private float m_timeToNextDrag;
    private float m_timeToNextSwipe;
    
    [Range(0.05f,1f)]
    public float m_minTimeToDrag = 0.15f;

    [Range(0.05f, 1f)]
    public float m_minTimeToSwipe = 0.3f;
    
    private bool m_didTap = false;

    enum Direction
    {
        None,
        Left,
        Right,
        Up,
        Down
    }

    private Direction m_dragDirection = Direction.None;
    private Direction m_swipeDirection = Direction.None;

    private void OnEnable()
    {
        TouchManager.DragEvent += DragHandler;
        TouchManager.SwipeEvent += SwipeHandler;
        TouchManager.TapEvent += TapHandler;
    }

    private void OnDisable()
    {
        TouchManager.DragEvent -= DragHandler;
        TouchManager.SwipeEvent -= SwipeHandler;
        TouchManager.TapEvent -= TapHandler;
    }

    void Start()
    {
        m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;
        m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;
        m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;

        //m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
        //m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

        m_gameBoard = GameObject.FindObjectOfType<Board>();
        m_spawner = GameObject.FindObjectOfType<Spawner>();
        m_soundManager = FindObjectOfType<SoundManager>();
        m_scoreManager = FindObjectOfType<ScoreManager>();
        m_ghost = FindObjectOfType<Ghost>();
        m_holder = FindObjectOfType<Holder>();

        if (!m_gameBoard)
        {
            Debug.LogWarning("WARNING: There is no game board defined!");
        }
        
        if (!m_soundManager)
        {
            Debug.LogWarning("WARNING: There is no sound manager defined!");
        }
        
        if (!m_scoreManager)
        {
            Debug.LogWarning("WARNING: There is no score manager defined!");
        }
        
        if (!m_ghost)
        {
            Debug.LogWarning("WARNING: There is no ghost defined!");
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

        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(false);
        }

        if (m_pausePanel)
        {
            m_pausePanel.SetActive(false);
        }

        m_dropIntervalModded = m_dropInterval;
    }

    void Update()
    {
        if (!m_gameBoard || !m_spawner || !m_aciveShape || m_gameOver || !m_soundManager || !m_scoreManager)
        {
            return;
        }

        PlayerInput();
    }

    private void LateUpdate()
    {
        if (m_ghost)
        {
            m_ghost.DrawGhost(m_aciveShape, m_gameBoard);
        }
    }

    private void PlayerInput() 
    {
        if ((Input.GetButton("MoveRight") && (Time.time > m_timeToNextKeyLeftRight)) || Input.GetButtonDown("MoveRight"))
        {
            MoveRight();
        }
        else if ((Input.GetButton("MoveLeft") && (Time.time > m_timeToNextKeyLeftRight)) || Input.GetButtonDown("MoveLeft"))
        {
            MoveLeft();
        }
        else if (Input.GetButtonDown("Rotate") && (Time.time > m_timeToNextKeyRotate))
        {
            Rotate();
        }
        else if ((Input.GetButton("MoveDown") && (Time.time > m_timeToNextKeyDown)) || (Time.time > m_timeToDrop))
        {
            MoveDown();
        }
        else if ( (m_swipeDirection == Direction.Right && Time.time > m_timeToNextSwipe) || 
                  (m_dragDirection == Direction.Right && Time.time > m_timeToNextDrag))
        {
            MoveRight();
            m_timeToNextDrag = Time.time + m_minTimeToDrag;
            m_timeToNextSwipe = Time.time + m_minTimeToSwipe;
        }
        else if ( (m_swipeDirection == Direction.Left && Time.time > m_timeToNextSwipe) ||
                  (m_dragDirection == Direction.Left && Time.time > m_timeToNextDrag))
        {
            MoveLeft();
            m_timeToNextDrag = Time.time + m_minTimeToDrag;
            m_timeToNextSwipe = Time.time + m_minTimeToSwipe;
        }
        else if ((m_swipeDirection == Direction.Up && Time.time > m_timeToNextSwipe) || (m_didTap))
        {
            Rotate();
            m_timeToNextSwipe = Time.time + m_minTimeToSwipe;
            m_didTap = false;
        }
        else if (m_dragDirection == Direction.Down && Time.time > m_timeToNextDrag)
        {
            MoveDown();
        }
        else if (Input.GetButtonDown("ToggleRot"))
        {
            ToggleRotDirection();
        }
        else if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }
        else if (Input.GetButtonDown("Hold"))
        {
            Hold();
        }
        
        m_dragDirection = Direction.None;
        m_swipeDirection = Direction.None;
        m_didTap = false;
    }

    private void MoveDown()
    {
        m_timeToDrop = Time.time + m_dropIntervalModded;
        m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;

        m_aciveShape.MoveDown();

        if (!m_gameBoard.IsValidPosition(m_aciveShape))
        {
            if (m_gameBoard.IsOverLimit(m_aciveShape))
            {
                GameOver();
            }
            else
            {
                LandShape();
            }
        }
    }

    private void Rotate()
    {
        m_aciveShape.RotateClockWise(m_clockwise);
        m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;

        if (!m_gameBoard.IsValidPosition(m_aciveShape))
        {
            m_aciveShape.RotateClockWise(!m_clockwise);
            PlaySound(m_soundManager.m_errorSound, 0.5f);
        }
        else
        {
            PlaySound(m_soundManager.m_moveSound, 0.5f);
        }
    }

    private void MoveLeft()
    {
        m_aciveShape.MoveLeft();
        m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;

        if (!m_gameBoard.IsValidPosition(m_aciveShape))
        {
            m_aciveShape.MoveRight();
            PlaySound(m_soundManager.m_errorSound, 0.5f);
        }
        else
        {
            PlaySound(m_soundManager.m_moveSound, 0.5f);
        }
    }

    private void MoveRight()
    {
        m_aciveShape.MoveRight();
        m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;

        if (!m_gameBoard.IsValidPosition(m_aciveShape))
        {
            m_aciveShape.MoveLeft();
            PlaySound(m_soundManager.m_errorSound, 0.5f);
        }
        else
        {
            PlaySound(m_soundManager.m_moveSound, 0.5f);
        }
    }

    private void GameOver()
    {
        m_aciveShape.MoveUp();
        m_gameOver = true;
        Debug.LogWarning(m_aciveShape.name + " is over the limit!");

        StartCoroutine(GameOverRoutine());
        
        PlaySound(m_soundManager.m_gameOverVocalClip, 5f);
        PlaySound(m_soundManager.m_gameOverSound, 5f);
    }

    private IEnumerator GameOverRoutine()
    {
        if (m_gameOverFx)
        {
            m_gameOverFx.Play();
        }

        yield return new WaitForSeconds(0.3f);
        
        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(true);
        }
    }

    private void LandShape()
    {
        m_timeToNextKeyLeftRight = Time.time;
        m_timeToNextKeyDown = Time.time;
        m_timeToNextKeyRotate = Time.time;
        
        if (m_aciveShape)
        {
            m_aciveShape.MoveUp();
            m_gameBoard.StoreShapeInGrid(m_aciveShape);
            
            m_aciveShape.LandShapeFX();

            if (m_ghost)
            {
                m_ghost.Reset();
            }

            if (m_holder)
            {
                m_holder.canRelease = true;
            }
            
            m_aciveShape = m_spawner.SpawnShape();
            
            m_gameBoard.StartCoroutine("ClearAllRows");

            PlaySound(m_soundManager.m_dropSound, 0.75f);

            if (m_gameBoard.m_completedRows > 0)
            {
                m_scoreManager.ScoreLines(m_gameBoard.m_completedRows);

                if (m_scoreManager.m_didLevelUp)
                {
                    PlaySound(m_soundManager.m_levelUpVocalClip, 0.5f);

                    m_dropIntervalModded = Mathf.Clamp(m_dropInterval - ((m_scoreManager.m_level - 1 ) * 0.05f), 0.05f, 1f);
                }
                else
                {
                    if (m_gameBoard.m_completedRows > 1)
                    {
                        AudioClip randomClip = m_soundManager.GetRandomClip(m_soundManager.m_vocalClips);
                        PlaySound(randomClip, 0.5f);
                    }
                }

                PlaySound(m_soundManager.m_clearRowSound);
            }
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Restarted");
    }

    private void PlaySound(AudioClip clip, float volMultiplier = 1)
    {
        if (m_soundManager.m_fxEnabled && clip)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Clamp(m_soundManager.m_fxVolume * volMultiplier, 0.05f, 1f));
        }
    }

    public void ToggleRotDirection()
    {
        m_clockwise = !m_clockwise;
        
        if (m_rotIconTogle)
        {
            m_rotIconTogle.ToggleIcon(m_clockwise);
        }
    }

    public void TogglePause()
    {
        if (m_gameOver)
        {
            return;
        }

        m_isPaused = !m_isPaused;

        if (m_pausePanel)
        {
            m_pausePanel.SetActive(m_isPaused);

            if (m_soundManager)
            {
                m_soundManager.m_musicSource.volume = (m_isPaused) ? m_soundManager.m_musicVolume * 0.25f : m_soundManager.m_musicVolume;
            }

            Time.timeScale = (m_isPaused) ? 0 : 1;
        }
    }

    public void Hold()
    {
        if (!m_holder)
        {
            return;
        }

        if (!m_holder.heldShape)
        {
            m_holder.Catch(m_aciveShape);
            m_aciveShape = m_spawner.SpawnShape();
            PlaySound(m_soundManager.m_holdSound);
        }
        else if (m_holder.canRelease)
        {
            Shape temp = m_aciveShape;
            m_aciveShape = m_holder.Release();
            m_aciveShape.transform.position = m_spawner.transform.position;
            m_holder.Catch(temp);
            PlaySound(m_soundManager.m_holdSound);
        }
        else
        {
            Debug.LogWarning("HOLDER WARNING: Wait for cool down!");
            PlaySound(m_soundManager.m_errorSound);
        }

        if (m_ghost)
        {
            m_ghost.Reset();
        }
    }

    private void DragHandler(Vector2 swipeMovement)
    {
        m_dragDirection = GetDirection(swipeMovement);
    }
    
    private void SwipeHandler(Vector2 swipeMovement)
    {
        m_swipeDirection = GetDirection(swipeMovement);
    }
    
    private void TapHandler(Vector2 swipeMovement)
    {
        m_didTap = true;
    }

    private Direction GetDirection(Vector2 swipeMovement)
    {
        Direction swipeDir = Direction.None;

        if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
        {
            swipeDir = swipeMovement.x >= 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            swipeDir = swipeMovement.y >= 0 ? Direction.Up : Direction.Down;
        }
        
        return swipeDir;
    }
}