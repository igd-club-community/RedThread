using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaner1Script : ActingPerson
{
    public PersonAct talkWithSecretary;
    public PersonAct putsHandfulOfSeeds;
    public PersonAct washTheShelf;
    public PersonAct bringPapersFrom2level;
    public PersonAct deliverPapers;
    public PersonAct fixBossGrass;
    public PersonAct fixCoolerOn1Level;
    public PersonAct fixCoolerOn2Level;
    public PersonAct fixCoolerOn3Level;

    public GameObject shelvesOnSecretaryTable;

    private Level1Controller levelController;

    public bool askedToBringPapers = false;

    new void Start()
    {
        base.Start();
        levelController = FindObjectOfType<Level1Controller>();
        //levelController.SecretaryIsBack += doTalkWithSecretary;
        //levelController.BossNeedsCoffee += doPutsHandfulOfSeeds;
        //levelController.BossNeedsPapers += doPutsHandfulOfSeeds;
        levelController.CleanerBringPapers += doRememberToBringPapersFrom2level;

        setAction(talkWithSecretary);
        currentAction = talkWithSecretary;
    }

    new void Update()
    {
        //base.Update();
        //noAction = false;
        //distance = Vector3.Distance(currentAction.target.position, transform.position);
        ////Если сломалось растение или один из кулеров а у нас задача принести бумагу, то после ремонта надо таки принести бумагу
        //if (!levelController.GrassInBossRoomIsFine)
        //{
        //    if (currentAction == fixBossGrass && distance < 1)
        //        levelController.GrassInBossRoomIsFine = true;
        //    else 
        //        doFixBossGrass();
        //}
        //else if (!levelController.CoolerOnFirstFloorIsFine)
        //{
        //    if (currentAction == fixCoolerOn1Level && distance < 1)
        //        levelController.CoolerOnFirstFloorIsFine = true;
        //    else
        //        doFixCoolerOn1Level();
        //}
        //else if (!levelController.CoolerOnSecondFloorIsFine)
        //{
        //    if (currentAction == fixCoolerOn2Level && distance < 1)
        //        levelController.CoolerOnSecondFloorIsFine = true;
        //    else
        //        doFixCoolerOn2Level();
        //}
        //else if (!levelController.CoolerOnThirdFloorIsFine)
        //{
        //    if (currentAction == fixCoolerOn3Level && distance < 1)
        //        levelController.CoolerOnThirdFloorIsFine = true;
        //    else
        //        doFixCoolerOn3Level();
        //}
        //else if (askedToBringPapers)
        //{
        //    if (currentAction == bringPapersFrom2level)
        //    {
        //        if (distance < 1)
        //            doDeliverPapers();
        //    }
        //    else if (currentAction == deliverPapers)
        //    {
        //        if (distance < 1)
        //        {
        //            levelController.generatePapersToPrinterDelivered();
        //            askedToBringPapers = false;
        //        }
        //    }
        //    else
        //        doBringPapersFrom2level();
        //}
        //else if (!levelController.SecretaryIsBisy)
        //    doTalkWithSecretary();
        //else if (levelController.MusorSpawned || levelController.MusorInInvertory || levelController.MusorOnBossKarniz)
        //    doWashTheShelf(); //стабильное состояние где уборщица может находиться сколько угодно долго, пока секретарша не освободится
        //else if (currentAction == putsHandfulOfSeeds && distance < 1)
        //{
        //    shelvesOnSecretaryTable.SetActive(true);
        //    levelController.MusorSpawned = true; //После этого вступит в силу условие с мытьем полок
        //}
        //else
        //{
        //    doPutsHandfulOfSeeds();
        //}
    }

    public void doTalkWithSecretary()
    {
        //Debug.Log("doTalkWithSecretary");
        //if (distance > 2)
        //    setAction(talkWithSecretary);
        //else
        //    noAction = true;
    }
    public void doPutsHandfulOfSeeds()
    {
        //Debug.Log("doPutsHandfulOfSeeds");
        setAction(putsHandfulOfSeeds);
    }
    public void doWashTheShelf()
    {
        levelController.CleanerIsBisy = false;
        //Debug.Log("doWashTheShelf");
        setAction(washTheShelf);
    }
    public void doRememberToBringPapersFrom2level()
    {
        Debug.Log("doRememberToBringPapersFrom2level");
        askedToBringPapers = true;
    }
    public void doBringPapersFrom2level()
    {
        Debug.Log("doBringPapersFrom2level");
        setAction(bringPapersFrom2level);
    }
    public void doDeliverPapers()
    {
        Debug.Log("doDeliverPapers");
        setAction(deliverPapers);
    }
    public void doFixBossGrass()
    {
        levelController.CleanerIsBisy = true;
        Debug.Log("doFixBossGrass");
        setAction(fixBossGrass);
    }
    public void doFixCoolerOn1Level()
    {
        levelController.CleanerIsBisy = true;
        Debug.Log("doFixCoolerOn1Level");
        setAction(fixCoolerOn1Level);
    }
    public void doFixCoolerOn2Level()
    {
        levelController.CleanerIsBisy = true;
        Debug.Log("doFixCoolerOn2Level");
        setAction(fixCoolerOn2Level);
    }
    public void doFixCoolerOn3Level()
    {
        levelController.CleanerIsBisy = true;
        Debug.Log("doFixCoolerOn3Level");
        setAction(fixCoolerOn3Level);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("2 floor"))
            levelController.CleanerOn2floor = true;
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("2 floor"))
            levelController.CleanerOn2floor = false;
    }
}
