using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroScript : ItemScript
{
    public Level1States states;
    private LevelController levelController;

    new void Start()
    {
        base.Start();
        levelController = FindObjectOfType<LevelController>();
    }

    public override void Act()
    {
        Debug.Log("ElectroScript");

        states.Electropower = !states.Electropower;
        levelController.generatePowerOff();
        
    }
}
