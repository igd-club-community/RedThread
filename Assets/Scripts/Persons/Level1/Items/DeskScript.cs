using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskScript : ItemScript
{
    public Level1States states;

    public override void Act()
    {
        Debug.Log("DeskScript");

        states.ProgrammerDeskClear = !states.ProgrammerDeskClear;
    }
}
