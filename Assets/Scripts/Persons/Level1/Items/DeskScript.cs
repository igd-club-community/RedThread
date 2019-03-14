using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskScript : ItemScript
{
    private Level1Controller levelController;

    new void Start()
    {
        base.Start();
        levelController = FindObjectOfType<Level1Controller>();
    }
    public override void Act()
    {
        Debug.Log("DeskScript");

        levelController.ProgrammerDeskClear = !levelController.ProgrammerDeskClear;
    }
}
