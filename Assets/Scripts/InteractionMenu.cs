using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool isMouseOver;
    
    [System.NonSerialized]
    public bool isMouseOverInteractiveObject;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!(isMouseOver || isMouseOverInteractiveObject))
                gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exited");
        isMouseOver = false;
    }

    void OnMouseExit()
    {
        Debug.Log("Mouse exited");
        isMouseOver = false;
    }
    void OnMouseOver()
    {
        isMouseOver = true;
    }
}
