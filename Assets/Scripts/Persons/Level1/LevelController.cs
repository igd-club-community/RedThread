using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    public delegate void PersonEvent();

    public event PersonEvent BossNeedsCoffee;
    public event PersonEvent BossNeedsPapers;
    public event PersonEvent CleanerBringPapers;
    public event PersonEvent CleanerDeliverPapers;
    public event PersonEvent CoffeeDelivered;
    public event PersonEvent SecretaryIsBack;
    public event PersonEvent PowerOff;
    public event PersonEvent PowerOn;
    public event PersonEvent PigeonsInBossRoom;

    public bool SecretaryOn2floor;
    public bool ProgrammerOn2floor;
    public bool BossOn2floor;
    public bool CleanerOn2floor;

    Level1States states;

    // Start is called before the first frame update
    void Start()
    {
        states = FindObjectOfType<Level1States>();
    }

    public void generateNeedCoffeEvent()
    {
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
    public void generateSecretaryIsBack()
    {
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
        Debug.Log("generatePapersDelivered");
        CleanerDeliverPapers();
    }
    public void generatePowerOff()
    {
        states.Electropower = false;
        Debug.Log("generatePowerOff");
        PowerOff();
    }
    public void generatePowerOn()
    {
        states.Electropower = true;
        Debug.Log("generatePowerOn");
        //PowerOn();
    }
    public void generatePigeonsInBossRoom()
    {
        Debug.Log("generatePigeonsInBossRoom");
        //PigeonsInBossRoom();
        states.PigeonsInBossRoom = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (SecretaryOn2floor && ProgrammerOn2floor && !BossOn2floor && !CleanerOn2floor)
            FindObjectOfType<LevelsLoader>().LoadCredits();
    }

}
