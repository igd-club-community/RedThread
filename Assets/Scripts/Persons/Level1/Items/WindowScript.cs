using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowScript : ItemScript
{
    public Level1States states;

    public override void Act()
    {
        Debug.Log("WindowScript");

        states.BossWindowBroken = !states.BossWindowBroken;
    }
}
