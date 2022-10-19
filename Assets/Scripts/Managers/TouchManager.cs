using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public delegate void TouchEventHandler(Vector2 swipe);

    public static event TouchEventHandler SwipeEvent;

    private Vector2 m_touchMovement;

    private void OnSwipe()
    {
        if (SwipeEvent != null)
        {
            SwipeEvent(m_touchMovement);
        }
    }
}