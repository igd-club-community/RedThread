using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassScript : ItemScript
{
    public Level1States states;

    public override void Act()
    {
        Debug.Log("GrassScript");

        states.GrassInBossRoomIsFine = !states.GrassInBossRoomIsFine;
    }
}
