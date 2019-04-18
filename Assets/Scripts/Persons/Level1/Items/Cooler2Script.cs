using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooler2Script : ItemScript
{
    private Level1Controller levelController;

    new void Start()
    {
        base.Start();
        levelController = FindObjectOfType<Level1Controller>();
    }
    public override void Act()
    {
        Debug.Log("Cooler2Script");
        this.GetComponent<FMODUnity.StudioEventEmitter>().Play();
        levelController.CoolerOnSecondFloorIsFine = false;
    }
}
