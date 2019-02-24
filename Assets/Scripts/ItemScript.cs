using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemScript : MonoBehaviour
{
    public abstract void Act();
    private Color storedColor;
    private Renderer materialRenderer;

    public void Start()
    {
        materialRenderer = gameObject.GetComponent<Renderer>();
        storedColor = materialRenderer.material.color;
    }

    public void onHover()
    {
        materialRenderer.material.color = Color.red;
    }

    public void deselect()
    {
        materialRenderer.material.color = storedColor;
    }
}
