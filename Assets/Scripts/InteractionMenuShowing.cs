using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionMenuShowing : MonoBehaviour
{
    public GameObject InteractionMenu;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            InteractionMenu interactionMenu = InteractionMenu.GetComponent<InteractionMenu>();
            if (Physics.Raycast(ray, out hit) && hit.collider.name == gameObject.name)
            {
                if (!InteractionMenu.activeSelf)
                    InteractionMenu.transform.position = Input.mousePosition;
                InteractionMenu.SetActive(true);
                if (interactionMenu)
                    interactionMenu.isMouseOverInteractiveObject = true;
            }
            else
            {
                if (interactionMenu)
                    interactionMenu.isMouseOverInteractiveObject = false;
            }
        }
    }
}
