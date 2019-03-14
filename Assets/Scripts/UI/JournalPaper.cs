using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalPaper : MonoBehaviour
{
    private void Update()
    {
        HideIfClickedOutside(gameObject);
    }

    public void CloseJournalPaper()
    {
        gameObject.SetActive(false);
    }

    private void HideIfClickedOutside(GameObject panel)
    {
        if (Input.GetMouseButton(0) && panel.activeSelf &&
            !RectTransformUtility.RectangleContainsScreenPoint(
                panel.GetComponent<RectTransform>(),
                Input.mousePosition,
                null))
        {
            
            panel.SetActive(false);
        }
    }
}