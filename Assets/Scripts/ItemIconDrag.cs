using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemIconDrag : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public void OnDrag(PointerEventData data)
    {
        transform.position = data.pointerCurrentRaycast.screenPosition;
    }

    public void OnEndDrag(PointerEventData data)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(0, 0, 0);
    }
}