using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaner1Script : MonoBehaviour
{
    LevelController levelController;

    public PersonAct currentAction;
    public PersonAct talkWithSecretary;
    public PersonAct putsHandfulOfSeeds;
    public PersonAct washTheShelf;

    private ActingPerson actingPerson;
    void Start()
    {
        actingPerson = GetComponent<ActingPerson>();
        levelController = FindObjectOfType<LevelController>();
        levelController.SecretaryIsBack += doTalkWithSecretary;
        levelController.BossNeedsCoffee += doPutsHandfulOfSeeds;

        actingPerson.setAction(talkWithSecretary);
        currentAction = talkWithSecretary;
    }
    
    void Update()
    {
        float distance;
        if (currentAction == putsHandfulOfSeeds)
        {
            distance = Vector3.Distance(currentAction.target.position, transform.position);
            if (distance < 1)
            {
                doWashTheShelf();
            }
        }
    }

    public void doTalkWithSecretary()
    {
        actingPerson.setAction(talkWithSecretary);
        currentAction = talkWithSecretary;
    }
    public void doPutsHandfulOfSeeds()
    {
        actingPerson.setAction(putsHandfulOfSeeds);
        currentAction = putsHandfulOfSeeds;
    }
    public void doWashTheShelf()
    {
        actingPerson.setAction(washTheShelf);
        currentAction = washTheShelf;
    }


}
