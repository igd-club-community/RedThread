using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassScript : ItemScript
{
    private Level1Controller levelController;

    new void Start()
    {
        base.Start();
        levelController = FindObjectOfType<Level1Controller>();
    }
    public override void Act()
    {

        //трава просыпается только если босс в порядке иначе это не имеет смысла
        if (!levelController.BossIsBisy && levelController.BossInBossRoom)
        {
            Debug.Log("GrassScript");
            levelController.GrassInBossRoomIsFine = false;
        }

    }
}
