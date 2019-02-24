using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarnizScript : ItemScript
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
        Debug.Log("KarnizScript");
        if (states.MusorInInvertory)
        {
            states.MusorOnBossKarniz = true;
            states.MusorInInvertory = false;
            levelController.generatePigeonsInBossRoom();
        }
    }
}
