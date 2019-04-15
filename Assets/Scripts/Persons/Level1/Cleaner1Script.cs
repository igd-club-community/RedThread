using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaner1Script : ActingPerson
{
    public PersonAct talkWithSecretary;
    public PersonAct putsHandfulOfSeeds;
    public PersonAct washTheShelf;
    public PersonAct talkWithBoss;
    public PersonAct bringPapersFrom2level;
    public PersonAct deliverPapers;
    public PersonAct somethingHappened;
    public PersonAct fixBossGrass;
    public PersonAct blockBoss;
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
        levelController.BossNeedsPapers += doTalkWithBoss;
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
            if (!levelController.GrassInBossRoomIsFine || !levelController.CoolerOnFirstFloorIsFine ||
                !levelController.CoolerOnSecondFloorIsFine || !levelController.CoolerOnThirdFloorIsFine)
            {
                levelController.CleanerIsBisy = true;
                setAction(somethingHappened);
            }
        }
        if (currentAction == washTheShelf && !levelController.SecretaryIsBisy)
        {
            doTalkWithSecretary();
        }

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
        else if (askedToBringPapers)
        {
            doBringPapersFrom2level();
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
        if (currentAction == blockBoss)
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
        else if (currentAction == fixCoolerOn3Level)
        {
            levelController.CoolerOnThirdFloorIsFine = true;
        }
        else if (currentAction == putsHandfulOfSeeds)
        {
            shelvesOnSecretaryTable.SetActive(true);
            levelController.MusorSpawned = true; //После этого вступит в силу условие с мытьем полок
        }
        //else if (currentAction == blockBoss)
        //{
        //    levelController.FloorIsWetInBossRoom = false;
        //}
    }
    protected override void goToNextAction()
    {
        base.goToNextAction();
        ////Если сломалось растение или один из кулеров а у нас задача принести бумагу, то после ремонта надо таки принести бумагу
        if (currentAction == somethingHappened)
        {
            problemSelector();
        }
        else if (currentAction == fixBossGrass)
        {
            //ждать пока мокрое высохнет и никого не пускать
            //levelController.FloorIsWetInBossRoom = true;
            setAction(blockBoss);
        }
        else if (currentAction == blockBoss || currentAction == fixCoolerOn1Level || currentAction == fixCoolerOn2Level || currentAction == fixCoolerOn3Level)
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
        else if (currentAction == talkWithBoss)
        {
            levelController.generatePasswordRemembered();
            restSelector();
        }
        else if (currentAction == bringPapersFrom2level)
        {
            doDeliverPapers();
        }
        else if (currentAction == deliverPapers)
        {
            levelController.generatePapersToPrinterDelivered();
            askedToBringPapers = false;
            setAction(washTheShelf);
        }
    }

    public void doTalkWithSecretary()
    {
        Debug.Log("doTalkWithSecretary");
        setAction(talkWithSecretary);
        levelController.CleanerIsBisy = false;
    }
    public void doTalkWithBoss()
    {
        if (levelController.passwordRemembered)
        {
            Debug.Log("passwordRemembered. go to rest");
            restSelector();
        }
        else
        {
            Debug.Log("doTalkWithBoss");
            setAction(talkWithBoss);
        }
    }
    public void doPutsHandfulOfSeeds()
    {
        if (levelController.CleanerIsBisy)
            return;
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
        if (currentAction == washTheShelf)
        {
            doBringPapersFrom2level();
        }
    }
    public void doBringPapersFrom2level()
    {
        levelController.CleanerIsBisy = true;
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
        Debug.Log("doFixBossGrass");
        setAction(fixBossGrass);
    }
    public void doFixCoolerOn1Level()
    {
        Debug.Log("doFixCoolerOn1Level");
        setAction(fixCoolerOn1Level);
    }
    public void doFixCoolerOn2Level()
    {
        Debug.Log("doFixCoolerOn2Level");
        setAction(fixCoolerOn2Level);
    }
    public void doFixCoolerOn3Level()
    {
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
