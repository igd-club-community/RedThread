using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultScript : ItemScript
{
    public Level1States states;

    public override void Act()
    {
        Debug.Log("VaultScript");

        if (states.BossShelterPassIsKnown)
            states.BossShelterLocked = false;
    }
}
