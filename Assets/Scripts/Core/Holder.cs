using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour
{
    public Transform holderXForm;
    public Shape heldShape;

    private float scale = 0.5f;

    public void Catch(Shape shape)
    {
        if (heldShape)
        {
            Debug.LogWarning("HOLDER WARNING: Release a shape before trying to hold!");
            return;
        }

        if (!shape)
        {
            Debug.LogWarning("HOLDER WARNING: Invalid shape!");
            return;
        }

        if (holderXForm)
        {
            shape.transform.position = holderXForm.position + shape.m_queueOffset;
            shape.transform.localScale = Vector3.one * scale;
            //heldShape = shape;
        }
        else
        {
            Debug.LogWarning("HOLDER WARNING: Holder has no transform assigned!");
        }
    }
}
