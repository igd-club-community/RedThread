using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputController : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    GameObject selectedItem;
    GameObject prevSelectedItem;

    Level1Controller level1Controller;
    LevelsLoader LevelsLoader;

    public Texture2D cursorDef;
    public Texture2D cursorHand;
    public Texture2D cursorUse;

    private void Start()
    {
        level1Controller = FindObjectOfType<Level1Controller>();
        LevelsLoader = FindObjectOfType<LevelsLoader>();

        Cursor.SetCursor(cursorDef, Vector2.zero, CursorMode.Auto);
    }

    void Update()
    {
        if (level1Controller.state == GameState.playing)
        {
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

                    if (selectedItem.GetComponent<ItemScript>().canBePickedInInventory)
                        Cursor.SetCursor(cursorHand, Vector2.zero, CursorMode.Auto);
                    else
                        Cursor.SetCursor(cursorUse, Vector2.zero, CursorMode.Auto);

                }
                prevSelectedItem = selectedItem;
            }
            else
            {
                if (prevSelectedItem != null) {
                    prevSelectedItem.GetComponent<ItemScript>().deselect();
                    Cursor.SetCursor(cursorDef, Vector2.zero, CursorMode.Auto);
                }
            }
        }
        else if (level1Controller.state == GameState.win)
        {
            if (Input.anyKey)
                LevelsLoader.LoadCredits();
        }
        else if (level1Controller.state == GameState.loose)
        {
            if (Input.anyKey)
                LevelsLoader.LoadLevel1Vitaliy();
        }

    }
}
