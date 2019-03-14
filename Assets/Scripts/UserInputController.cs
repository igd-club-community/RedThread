using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputController : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    GameObject selectedItem;
    GameObject prevSelectedItem;

    private void Start()
    {

    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.W))
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("ActionItems")))
        {
            selectedItem = hit.collider.gameObject;
            if (prevSelectedItem != selectedItem && prevSelectedItem != null)
                prevSelectedItem.GetComponent<ItemScript>().deselect();

            if (Input.GetMouseButtonDown(0))
            {
                print(hit.collider.name);
                selectedItem.GetComponent<ItemScript>().Act();
            }
            else
            {
                selectedItem.GetComponent<ItemScript>().onHover();
            }
            prevSelectedItem = selectedItem;

        }
        else
        {
            if (prevSelectedItem != null) prevSelectedItem.GetComponent<ItemScript>().deselect();
        }
    }
}
