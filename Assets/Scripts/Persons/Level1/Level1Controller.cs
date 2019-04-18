﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Controller : MonoBehaviour
{
    public delegate void PersonEvent();

    //состояния связанные с предметами
    public bool BossCupFilled = false;
    public bool BossCupMoved;
    public bool MusorSpawned; //положила шелуху на стол уборщицы
    public bool MusorInInvertory;
    public bool PaperInPrinter = true;
    public bool PaperInBossRoom = true;
    public bool MusorOnBossKarniz;
    public bool ProgrammerDeskClear;
    public bool BossShelterLocked;
    public bool BossShelterPassIsKnown;
    public bool ElectropowerOn;
    public bool BossWindowBroken;
    public bool PigeonsInBossRoom;
    public bool GrassInBossRoomIsFine;
    public bool CoolerOnFirstFloorIsFine;
    public bool CoolerOnSecondFloorIsFine;
    public bool CoolerOnThirdFloorIsFine;
    public bool BossShelterEmpty;

    public event PersonEvent BossNeedsCoffee;
    public event PersonEvent BossNeedsPapers;
    public event PersonEvent BossNeedsToRepairWindow;
    public event PersonEvent CleanerBringPapers;
    public event PersonEvent CoffeeDelivered;
    public event PersonEvent PapersDelivered;
    public event PersonEvent SecretaryIsBack;
    public event PersonEvent PowerOff;
    public event PersonEvent PowerOn;
    public event PersonEvent PigeonsCameInBossRoom;

    //состояния связанные с персонажами
    public bool SecretaryIsBisy;
    public bool CleanerIsBisy;
    public bool SecretaryOn2floor;
    public bool ProgrammerOn2floor;
    public bool BossOn2floor;
    public bool CleanerOn2floor;
    public bool BossInBossRoom;
    public bool BossOffline;

    public GameObject pigeons;
    public ElectroScript FuseBox;

    // Start is called before the first frame update
    void Start()
    {

    }

    //Босс попросил секретаршу сделать кофе
    public void generateNeedCoffeEvent()
    {
        SecretaryIsBisy = true;
        Debug.Log("generateNeedCoffeEvent");
        BossNeedsCoffee();
    }

    //Босс попросил секретаршу напечатать бумаги
    public void generateNeedPapersEvent()
    {
        SecretaryIsBisy = true;
        Debug.Log("generateNeedPapersEvent");
        BossNeedsPapers();
    }

    //Секретарша принесла кофе
    public void generateCoffeeDelivered()
    {
        Debug.Log("generateCoffeeDelivered");
        CoffeeDelivered();
    }

    //Секретарша принесла бумаги
    public void generatePapersToBossDelivered()
    {
        PaperInBossRoom = true;
        Debug.Log("generatePapersToBossDelivered");
        //PapersDelivered();
    }

    //Босс просит секретаршу вызвать монтажников
    public void generateBossNeedsToRepairWindow()
    {
        Debug.Log("generateBossNeedsToRepairWindow");
        BossNeedsToRepairWindow();
    }


    public void generateSecretaryIsBack()
    {
        SecretaryIsBisy = false;
        Debug.Log("generateSecretaryIsBack");
        //SecretaryIsBack();
    }

    //Просьба секретарши к уборщице принести бумаги
    public void generateCleanerBringPapersEvent()
    {
        CleanerIsBisy = true;
        SecretaryIsBisy = false; //В принципе не обязательно, так как сразу секретарша пойдет к столу и станет свободна
        Debug.Log("generateCleanerBringPapersEvent");
        CleanerBringPapers();
    }

    //Просьба уборщица принесла бумаги
    public void generatePapersToPrinterDelivered()
    {
        CleanerIsBisy = false;
        SecretaryIsBisy = true;
        PaperInPrinter = true;
        Debug.Log("generatePapersToPrinterDelivered");
        PapersDelivered();
    }

    public void generatePowerOff()
    {
        Debug.Log("generatePowerOff");
        if (ElectropowerOn)
        {
            FuseBox.GetComponent<FMODUnity.StudioEventEmitter>().Play();
            ElectropowerOn = false; // !states.Electropower;
        }
    }
    public void generatePowerOn()
    {
        ElectropowerOn = true;
        Debug.Log("generatePowerOn");
        FuseBox.GetComponent<FMODUnity.StudioEventEmitter>().Play();
    }
    public void generatePigeonsInBossRoom()
    {
        Debug.Log("generatePigeonsInBossRoom");
        //PigeonsCameInBossRoom();
        if (!BossShelterLocked)
            BossShelterEmpty = true;
        PigeonsInBossRoom = true;
        pigeons.GetComponent<MeshRenderer>().enabled = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (SecretaryOn2floor && ProgrammerOn2floor && !BossOn2floor && !CleanerOn2floor)
            FindObjectOfType<LevelsLoader>().LoadCredits();
        if(checkLoose())
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Bad End", GetComponent<Transform>().position);
            //FindObjectOfType<LevelsLoader>().LoadCredits();
        }
    }
    bool checkLoose()
    {
        //TODO ADD УСЛОВИЯ ПОРАЖЕНИЯ
        return false;
    }
}
