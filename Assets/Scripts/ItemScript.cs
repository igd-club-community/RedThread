using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemScript : MonoBehaviour
{
    public abstract void Act();
    private Color storedColor;
    private Renderer materialRenderer;
    bool hovered = false;
    private bool hasMaterialRenderer = true;

    public void Start()
    {
        materialRenderer = gameObject.GetComponent<Renderer>();
        if (materialRenderer == null)
        {
            hasMaterialRenderer = false;
        }
    }

    public void onHover()
    {
        if (!hovered && hasMaterialRenderer)
        {
            storedColor = materialRenderer.material.color;
            materialRenderer.material.color = Color.red;
        }
        hovered = true;
    }

    public void deselect()
    {
        if (hasMaterialRenderer)
        {
            materialRenderer.material.color = storedColor;
        }
        hovered = false;
    }
}
