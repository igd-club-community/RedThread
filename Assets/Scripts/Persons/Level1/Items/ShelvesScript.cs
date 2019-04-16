using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelvesScript : ItemScript
{
    private Level1Controller levelController;
    public UIController UIController;

    new void Start()
    {
        base.Start();
        levelController = FindObjectOfType<Level1Controller>();
        UIController = FindObjectOfType<UIController>();
    }
    public override void Act()
    {
        Debug.Log("ShelvesScript");
        if (gameObject.activeSelf)
        {
            levelController.MusorSpawned = false;
            levelController.MusorInInvertory = true;
            UIController.shelvesIcon.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
