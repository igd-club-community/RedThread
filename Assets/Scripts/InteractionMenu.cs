using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// if something broke, uncomment interfaces lower and add OnPointerEnter, OnPointerExit
public class InteractionMenu : MonoBehaviour //, IPointerEnterHandler, IPointerExitHandler
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

    void OnMouseExit()
    {
        isMouseOver = false;
    }
    void OnMouseOver()
    {
        isMouseOver = true;
    }
    
}
