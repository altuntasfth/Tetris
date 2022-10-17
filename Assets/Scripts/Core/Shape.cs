using UnityEngine;

public class Shape : MonoBehaviour
{
    public bool m_isRotate = true;

    public Vector3 m_queueOffset;

    public string glowSquareTag = "LandShapeFx";
    private GameObject[] m_glowSquareFx;

    private void Start()
    {
        if (glowSquareTag != "")
        {
            m_glowSquareFx = GameObject.FindGameObjectsWithTag(glowSquareTag);
        }
    }

    public void LandShapeFX()
    {
        int i = 0;

        foreach (Transform child in gameObject.transform)
        {
            if (m_glowSquareFx[i])
            {
                m_glowSquareFx[i].transform.position = new Vector3(child.position.x, child.position.y, -2f);
                ParticlePlayer particlePlayer = m_glowSquareFx[i].GetComponent<ParticlePlayer>();

                if (particlePlayer)
                {
                    particlePlayer.Play();
                }
            }

            i++;
        }
    }

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
