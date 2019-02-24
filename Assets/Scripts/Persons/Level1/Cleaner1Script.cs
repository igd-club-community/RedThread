using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaner1Script : MonoBehaviour
{
    public PersonAct currentAction;
    public PersonAct talkWithSecretary;
    public PersonAct putsHandfulOfSeeds;
    public PersonAct washTheShelf;
    public PersonAct bringPapersFrom2level;
    public PersonAct deliverPapers;
    public float distance;

    public GameObject shelvesOnSecretaryTable;

    private ActingPerson actingPerson;
    private LevelController levelController;
    Level1States states;

    void Start()
    {
        actingPerson = GetComponent<ActingPerson>();
        levelController = FindObjectOfType<LevelController>();
        levelController.SecretaryIsBack += doTalkWithSecretary;
        levelController.BossNeedsCoffee += doPutsHandfulOfSeeds;
        levelController.BossNeedsPapers += doPutsHandfulOfSeeds;
        levelController.CleanerBringPapers += doBringPapersFrom2level;
        states = FindObjectOfType<Level1States>();

        actingPerson.setAction(talkWithSecretary);
        currentAction = talkWithSecretary;
    }
    
    void Update()
    {
        distance = Vector3.Distance(currentAction.target.position, transform.position);
        if (currentAction == putsHandfulOfSeeds)
        {
            if (distance < 1)
            {
                shelvesOnSecretaryTable.SetActive(true);
                states.MusorSpawned = true;
                doWashTheShelf();
            }
        }
        else if (currentAction == bringPapersFrom2level && distance < 1)
        {
            doDeliverPapers();
        }
        else if (currentAction == deliverPapers && distance < 1)
        {
            states.PaperInPrinter = true;
            levelController.generatePapersDelivered();
            doPutsHandfulOfSeeds();
        }
    }

    public void doTalkWithSecretary()
    {
        if (currentAction != bringPapersFrom2level)
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
    public void doBringPapersFrom2level()
    {
        Debug.Log("doBringPapersFrom2level");
        actingPerson.setAction(bringPapersFrom2level);
        currentAction = bringPapersFrom2level;
    }
    public void doDeliverPapers()
    {
        Debug.Log("doDeliverPapers");
        actingPerson.setAction(deliverPapers);
        currentAction = deliverPapers;
    }

}
