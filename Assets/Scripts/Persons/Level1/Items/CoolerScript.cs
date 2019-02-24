using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolerScript : ItemScript
{
    public Level1States states;

    public override void Act()
    {
        Debug.Log("CoolerScript");

        states.CoolerOnFirstFloor = !states.CoolerOnFirstFloor;
    }
}
