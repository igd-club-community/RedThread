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
    public bool PaperInBossRoom = true;
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
    public bool passwordRemembered = false;
    public bool documentsStolen = false;
    //public bool FloorIsWetInBossRoom = false;

    public event PersonEvent BossNeedsCoffee;
    public event PersonEvent BossNeedsPapers;
    public event PersonEvent PasswordRemembered;
    public event PersonEvent BossNeedsToRepairWindow;
    public event PersonEvent CleanerBringPapers;
    public event PersonEvent CoffeeDelivered;
    public event PersonEvent PapersDelivered;
    //public event PersonEvent SecretaryIsBack;
    public event PersonEvent SecretaryCanGoToDesk;
    public event PersonEvent PowerOff;
    public event PersonEvent PowerOn;
    public event PersonEvent PigeonsCameInBossRoom;
    public event PersonEvent EndDialogWithBoss;

    //состояния связанные с положением персонажей
    public bool SecretaryIsBisy;
    public bool CleanerIsBisy;
    public bool SecretaryOn2floor;
    public bool ProgrammerOn2floor;
    public bool BossOn2floor;
    public bool CleanerOn2floor;
    public bool BossInBossRoom;
    public bool BossIsBisy;

    public GameObject pigeons;
    public GameObject window;

    UIController uIController;
    public GameState state = GameState.playing;

    // Start is called before the first frame update
    void Start()
    {
        uIController = FindObjectOfType<UIController>();
    }

    //Босс попросил секретаршу сделать кофе
    public void generateNeedCoffeEvent()
    {
        SecretaryIsBisy = true;
        Debug.Log("generateNeedCoffeEvent");
        BossNeedsCoffee();
    }

    public void generateSecretaryCanGoToDesk()
    {
        Debug.Log("generateSecretaryCanGoToDesk");
        SecretaryCanGoToDesk();
    }

    //Босс попросил секретаршу напечатать бумаги
    public void generateNeedPapersEvent()
    {
        SecretaryIsBisy = true;
        Debug.Log("generateNeedPapersEvent");
        BossNeedsPapers();
    }

    //Босс попросил секретаршу напечатать бумаги
    public void generatePasswordRemembered()
    {
        Debug.Log("generatePasswordRemembered");
        PasswordRemembered();
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


    //public void generateSecretaryIsBack()
    //{
    //    Debug.Log("generateSecretaryIsBack");
    //    //SecretaryIsBack();
    //}

    //Просьба секретарши к уборщице принести бумаги
    public void generateCleanerBringPapersEvent()
    {
        //CleanerIsBisy = true;
        //SecretaryIsBisy = false; //В принципе не обязательно, так как сразу секретарша пойдет к столу и станет свободна
        Debug.Log("generateCleanerBringPapersEvent");
        CleanerBringPapers();
    }

    //Просьба уборщица принесла бумаги
    public void generatePapersToPrinterDelivered()
    {
        CleanerIsBisy = false;
        //SecretaryIsBisy = true;
        PaperInPrinter = true;
        Debug.Log("generatePapersToPrinterDelivered");
        PapersDelivered();
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
        if (!BossWindowBroken)
        {
            //здесь так же запускать звуки и анимацию ломания стекла
            window.GetComponent<MeshRenderer>().enabled = true;
            BossWindowBroken = true;
        }

        if (!BossShelterLocked)
        {
            generateDocumentStolen();
        }

        PigeonsInBossRoom = true;
        //вызывать менеджера голубей который запустит их анимацию.
        pigeons.SetActive(true);
    }
    public void generateDocumentStolen()
    {
        Debug.Log("generateDocumentStolen");

        documentsStolen = true;
        //вызывать менеджера голубей который запустит анимацию кражи
    }

    internal void generateBossSecrProgrEngDialog()
    {
        EndDialogWithBoss();
    }

    // Update is called once per frame
    void Update()
    {
        if (BossIsBisy && SecretaryOn2floor && ProgrammerOn2floor && !BossOn2floor && !CleanerOn2floor)
            //FindObjectOfType<LevelsLoader>().LoadCredits();
            win();
    }

    public void win()
    {
        uIController.enableWin();
        state = GameState.win;
    }
    public void loose()
    {
        uIController.enableLoose();
        state = GameState.loose;
    }

}

public enum GameState
{
    playing,
    win,
    loose
}
