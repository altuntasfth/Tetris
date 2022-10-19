using System;
using UnityEngine;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    public delegate void TouchEventHandler(Vector2 swipe);

    public static event TouchEventHandler SwipeEvent;

    public Text m_diagnosticText1;
    public Text m_diagnosticText2;
    public bool m_useDiagnostic;

    private Vector2 m_touchMovement;
    private int m_minSwipeDistance = 20;

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
                Diagnostic("", "");
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                m_touchMovement += touch.deltaPosition;

                if (m_touchMovement.magnitude > m_minSwipeDistance)
                {
                    OnSwipe();
                    Diagnostic("Swipe detected", m_touchMovement.ToString() + " " + SwipeDiagnostic(m_touchMovement));
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

    private void OnSwipe()
    {
        if (SwipeEvent != null)
        {
            SwipeEvent(m_touchMovement);
        }
    }
}