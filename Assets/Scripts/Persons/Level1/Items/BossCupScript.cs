using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCupScript : ItemScript
{
    private Level1Controller levelController;
    public GameObject soundEmitter;

    new void Start()
    {
        base.Start();
        levelController = FindObjectOfType<Level1Controller>();
    }
    public override void Act()
    {
        Debug.Log("BossCupScript");
        //Изменить положение чашки
        //Изменить состояние сцены
        if (levelController.BossCupFilled)
        {
            levelController.BossCupMoved = true;
            levelController.PaperInBossRoom = false;
        }
        FMODUnity.RuntimeManager.PlayOneShot("event:/OnClick", GetComponent<Transform>().position);
    }
}
