using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public bool m_isRotate = true;

    public Vector3 m_queueOffset;

    void Move(Vector3 direction)
    {
        transform.position += direction;
    }

    public void MoveDown()
    {
        Move(new Vector3(0, -1, 0));
    }
    
    public void MoveUp()
    {
        Move(new Vector3(0, 1, 0));
    }
    
    public void MoveLeft()
    {
        Move(new Vector3(-1, 0, 0));
    }
    
    public void MoveRight()
    {
        Move(new Vector3(1, 0, 0));
    }

    public void RotateLeft()
    {
        if (m_isRotate)
        {
            transform.Rotate(new Vector3(0, 0, 90));
        }
    }
    
    public void RotateRight()
    {
        if (m_isRotate)
        {
            transform.Rotate(new Vector3(0, 0, -90));
        }
    }

    public void RotateClockWise(bool clockwise)
    {
        if (clockwise)
        {
            RotateRight();
        }
        else
        {
            RotateLeft();
        }
    }
}
