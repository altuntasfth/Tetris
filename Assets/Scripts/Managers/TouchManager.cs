using System;
using UnityEngine;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    public delegate void TouchEventHandler(Vector2 swipe);

    public static event TouchEventHandler DragEvent;
    public static event TouchEventHandler SwipeEvent;
    public static event TouchEventHandler TapEvent;
    
    [Range(50,150)]
    public int m_minDragDistance = 100;

    [Range(20,250)]
    public int m_minSwipeDistance = 50;
    
    private float m_tapTimeMax = 0;
    public float m_tapTimeWindow = 0.1f;

    public Text m_diagnosticText1;
    public Text m_diagnosticText2;
    public bool m_useDiagnostic;

    private Vector2 m_touchMovement;

    private void Start()
    {
        Diagnostic("", "");
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            
            if (touch.phase == TouchPhase.Began)
            {
                m_touchMovement = Vector2.zero;
                m_tapTimeMax = Time.time + m_tapTimeWindow;
                Diagnostic("", "");
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                m_touchMovement += touch.deltaPosition;

                if (m_touchMovement.magnitude > m_minDragDistance)
                {
                    OnDrag();
                    Diagnostic("Drag detected", m_touchMovement.ToString() + " " + SwipeDiagnostic(m_touchMovement));
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (m_touchMovement.magnitude > m_minSwipeDistance)
                {
                    OnSwipeEnd();
                    Diagnostic("Swipe detected", m_touchMovement.ToString() + " " + SwipeDiagnostic(m_touchMovement));
                }
                else if (Time.time < m_tapTimeMax)
                {
                    OnTap();
                    Diagnostic("Tap detected",m_touchMovement.ToString() + " " + SwipeDiagnostic(m_touchMovement));
                }
            }
        }
    }

    private void Diagnostic(string text1, string text2)
    {
        m_diagnosticText1.gameObject.SetActive(m_useDiagnostic);
        m_diagnosticText2.gameObject.SetActive(m_useDiagnostic);

        if (m_diagnosticText1 && m_diagnosticText2)
        {
            m_diagnosticText1.text = text1;
            m_diagnosticText2.text = text2;
        }
    }

    private string SwipeDiagnostic(Vector2 swipeMovement)
    {
        string direction = "";

        if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
        {
            direction = swipeMovement.x >= 0 ? "right" : "left";
        }
        else
        {
            direction = swipeMovement.y >= 0 ? "up" : "down";
        }

        return direction;
    }

    private void OnDrag()
    {
        if (DragEvent != null)
        {
            DragEvent(m_touchMovement);
        }
    }
    
    private void OnSwipeEnd()
    {
        if (SwipeEvent != null)
        {
            SwipeEvent(m_touchMovement);
        }
    }
    
    private void OnTap()
    {
        if (TapEvent != null)
        {
            TapEvent(m_touchMovement);
        }
    }
}