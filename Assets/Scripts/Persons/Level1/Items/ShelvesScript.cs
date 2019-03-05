using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelvesScript : ItemScript
{
    private Level1Controller levelController;

    new void Start()
    {
        base.Start();
        levelController = FindObjectOfType<Level1Controller>();
    }
    public override void Act()
    {
        Debug.Log("ShelvesScript");
        if (gameObject.activeSelf)
        {
            levelController.MusorSpawned = false;
            levelController.MusorInInvertory = true;
            gameObject.SetActive(false);
        }
    }
}
