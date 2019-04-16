using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroScript : ItemScript
{
    private Level1Controller levelController;

    new void Start()
    {
        base.Start();
        levelController = FindObjectOfType<Level1Controller>();
    }
    public override void Act()
    {
        Debug.Log("ElectroScript");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Electricity", GetComponent<Transform>().position);
        levelController.Electropower = false; // !states.Electropower;
        levelController.generatePowerOff();
    }
}
