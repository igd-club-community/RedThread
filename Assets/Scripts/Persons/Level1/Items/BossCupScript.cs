using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCupScript : ItemScript
{
    public Level1States states;

    public override void Act()
    {
        Debug.Log("BossCupScript");
        //Изменить положение чашки
        //Изменить состояние сцены
        if (states.BossCupFilled)
            states.BossCupMoved = true;
    }
}
