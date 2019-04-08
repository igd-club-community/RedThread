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

    public Stack<PersonAct> rememberedActions = new Stack<PersonAct>();

    new void Start()
    {
        base.Start();
        levelController = FindObjectOfType<Level1Controller>();
        //levelController.SecretaryIsBack += doTalkWithSecretary; //не всегда. Если уборщица убирается гдето то она не должна реагировать на возврат секретарши
        levelController.BossNeedsCoffee += doPutsHandfulOfSeeds;
        levelController.BossNeedsPapers += doPutsHandfulOfSeeds;
        levelController.CleanerBringPapers += doRememberToBringPapersFrom2level;

        doTalkWithSecretary();

        name = "cleaner";
    }

    new void Update()
    {
        //Уборщица должна реагировать на каждое изменение в сцене. Даже стек не понадобится на самом деле
        //Если у нас стейт говорить с уборщицей или мыть полки,
        //То при появлении события что гдето пролита вода или появилась грязь убощица должна немедленно выдвигаться.
        base.Update();

        if (currentAction == talkWithSecretary || currentAction == washTheShelf)
        {
            problemSelector();
        }
        if (currentAction == washTheShelf && !levelController.SecretaryIsBisy)
        {
            doTalkWithSecretary();
        }



        //else if (askedToBringPapers)
        //{
        //    if (currentAction == bringPapersFrom2level)
        //    {
        //        doDeliverPapers();
        //    }
        //    else if (currentAction == deliverPapers)
        //    {
        //        levelController.generatePapersToPrinterDelivered();
        //        askedToBringPapers = false;
        //    }
        //    else
        //        doBringPapersFrom2level();
        //}
        //else if (!levelController.SecretaryIsBisy)
        //    doTalkWithSecretary();
    }

    public bool problemSelector()
    {
        if (!levelController.GrassInBossRoomIsFine)
        {
            //if (currentAction == fixBossGrass)
            //    levelController.GrassInBossRoomIsFine = true;
            //else
            doFixBossGrass();
            return true;
        }
        else if (!levelController.CoolerOnFirstFloorIsFine)
        {
            doFixCoolerOn1Level();
            return true;
        }
        else if (!levelController.CoolerOnSecondFloorIsFine)
        {
            doFixCoolerOn2Level();
            return true;
        }
        else if (!levelController.CoolerOnThirdFloorIsFine)
        {
            doFixCoolerOn3Level();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void restSelector()
    {
        if (levelController.SecretaryIsBisy)
            doWashTheShelf();
        else
            doTalkWithSecretary();
    }

    //задания закончившиеся естественны образом, т.е. циклы
    protected override void preFinishOfCurrentAction()
    {
        if (currentAction == fixBossGrass)
        {
            levelController.GrassInBossRoomIsFine = true;
        }
        else if (currentAction == fixCoolerOn1Level)
        {
            levelController.CoolerOnFirstFloorIsFine = true;
        }
        else if (currentAction == fixCoolerOn2Level)
        {
            levelController.CoolerOnSecondFloorIsFine = true;
        }
        if (currentAction == fixCoolerOn3Level)
        {
            levelController.CoolerOnThirdFloorIsFine = true;
        }
        else if (currentAction == putsHandfulOfSeeds)
        {
            shelvesOnSecretaryTable.SetActive(true);
            levelController.MusorSpawned = true; //После этого вступит в силу условие с мытьем полок
        }
    }
    protected override void goToNextAction()
    {
        base.goToNextAction();
        ////Если сломалось растение или один из кулеров а у нас задача принести бумагу, то после ремонта надо таки принести бумагу
        if (currentAction == fixBossGrass || currentAction == fixCoolerOn1Level || currentAction == fixCoolerOn2Level || currentAction == fixCoolerOn3Level)
        {
            if (!problemSelector())
            {
                restSelector();
            }
        }
        else if (currentAction == putsHandfulOfSeeds)
        {
            doWashTheShelf();
        }
    }

    public void doTalkWithSecretary()
    {
        Debug.Log("doTalkWithSecretary");
        setAction(talkWithSecretary);
        levelController.CleanerIsBisy = false;
    }
    public void doPutsHandfulOfSeeds()
    {
        Debug.Log("doPutsHandfulOfSeeds");
        if (levelController.MusorSpawned || levelController.MusorInInvertory || levelController.MusorOnBossKarniz) //или если окно разбито
            doWashTheShelf(); //стабильное состояние где уборщица может находиться сколько угодно долго, пока секретарша не освободится
        else 
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
