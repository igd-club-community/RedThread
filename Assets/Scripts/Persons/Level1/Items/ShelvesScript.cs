using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelvesScript : ItemScript
{
    public Level1States states;

    public override void Act()
    {
        Debug.Log("ShelvesScript");
        if (gameObject.activeSelf)
        {
            states.MusorSpawned = false;
            states.MusorInInvertory = true;
            gameObject.SetActive(false);
        }
    }
}
