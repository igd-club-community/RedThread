using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultScript : ItemScript
{
    private Level1Controller levelController;

    new void Start()
    {
        base.Start();
        levelController = FindObjectOfType<Level1Controller>();
    }
    public override void Act()
    {
        Debug.Log("VaultScript");

        if (levelController.BossShelterPassIsKnown)
            levelController.BossShelterLocked = false;
    }
}
