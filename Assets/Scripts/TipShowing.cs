using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemSlot))]
public class TipShowing : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Label;
    ItemSlot itemSlot;

    void Start()
    {
        itemSlot = GetComponent<ItemSlot>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Label && itemSlot.CurrentItem != Item.NoItem)
            Label.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Label)
            Label.SetActive(false);
    }
}
