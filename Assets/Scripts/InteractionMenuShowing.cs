using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionMenuShowing : MonoBehaviour
{
    public GameObject interactionMenu;

    private InteractionMenu _interactionMenu;

    private void Start()
    {
        _interactionMenu = interactionMenu.GetComponent<InteractionMenu>();
    }

    private void OnMouseDown()
    {
        if (!interactionMenu.activeSelf)
            interactionMenu.transform.position = Input.mousePosition;
        interactionMenu.SetActive(true);
        if (_interactionMenu)
            _interactionMenu.isMouseOverInteractiveObject = true;
    }

    private void OnMouseExit()
    {
        if (_interactionMenu)
            _interactionMenu.isMouseOverInteractiveObject = false;
    }
}
