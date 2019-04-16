using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarnizScript : ItemScript
{
    private Level1Controller levelController;
    public UIController UIController;
    public GameObject shelves;

    new void Start()
    {
        base.Start();
        levelController = FindObjectOfType<Level1Controller>();
        UIController = FindObjectOfType<UIController>();
    }

    public override void Act()
    {
        Debug.Log("KarnizScript");
        if (levelController.MusorInInvertory)
        {
            levelController.MusorOnBossKarniz = true;
            levelController.MusorInInvertory = false;
            UIController.shelvesIcon.SetActive(false);
            shelves.SetActive(true);
            levelController.PigeonsInBossRoom = true;
            if (!levelController.BossInBossRoom)
                levelController.generatePigeonsInBossRoom();
        }
    }
}
