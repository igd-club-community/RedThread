using System;
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
    public bool MusorOnBossKarniz;
    public bool ProgrammerDeskClear;
    public bool BossShelterLocked;
    public bool BossShelterPassIsKnown;
    public bool Electropower;
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

    // Start is called before the first frame update
    void Start()
    {

    }

    public void generateNeedCoffeEvent()
    {
        SecretaryIsBisy = true;
        Debug.Log("generateNeedCoffeEvent");
        BossNeedsCoffee();
    }
    public void generateNeedPapersEvent()
    {
        Debug.Log("generateNeedPapersEvent");
        BossNeedsPapers();
    }
    public void generateCoffeeDelivered()
    {
        Debug.Log("generateCoffeeDelivered");
        CoffeeDelivered();
    }
    public void generateBossNeedsToRepairWindow()
    {
        Debug.Log("generateBossNeedsToRepairWindow");
        BossNeedsToRepairWindow();
    }
    public void generateSecretaryIsBack()
    {
        SecretaryIsBisy = false;
        Debug.Log("generateSecretaryIsBack");
        SecretaryIsBack();
    }

    public void generateCleanerBringPapersEvent()
    {
        Debug.Log("generateCleanerBringPapersEvent");
        CleanerBringPapers();
    }
    public void generatePapersDelivered()
    {
        PaperInPrinter = true;
        Debug.Log("generatePapersDelivered");
    }
    public void generatePowerOff()
    {
        Electropower = false;
        Debug.Log("generatePowerOff");
        PowerOff();
    }
    public void generatePowerOn()
    {
        Electropower = true;
        Debug.Log("generatePowerOn");
        //PowerOn();
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
    }

}
