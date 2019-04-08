using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Programmer1Script : ActingPerson
{
    private Level1Controller levelController;

    public PersonAct workWithPC;
    public PersonAct watchOnDesk;
    public PersonAct writeOnDesk;
    public PersonAct watchOnCat;
    public PersonAct switchPowerOn;
    
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        levelController = FindObjectOfType<Level1Controller>();
        levelController.PowerOff += doSwitchPowerOn;

        doWorkWirkPC();
        name = "program";
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        noAction = false;
        if (levelController.BossOn2floor)
            noAction = true;


    }

    //задания закончившиеся естественны образом, т.е. циклы
    protected override void goToNextAction()
    {
        base.goToNextAction();
        if (currentAction == workWithPC)
        {
            doWatchOnDesk();
        }
        else if (currentAction == watchOnDesk)
        {
            doWriteOnDesk();

        }
        else if (currentAction == writeOnDesk)
        {
            doWatchOnCat();

        }
        else if (currentAction == watchOnCat)
        {
            doWorkWirkPC();
        }
        else if (currentAction == switchPowerOn)
        {
            levelController.generatePowerOn();
            doWorkWirkPC();
        }
    }

    void doWorkWirkPC()
    {
        Debug.Log("doWorkWirkPC");
        setAction(workWithPC);
    }
    void doWatchOnDesk()
    {
        setAction(watchOnDesk);
    }
    void doWriteOnDesk()
    {
        setAction(writeOnDesk);
    }
    void doWatchOnCat()
    {
        setAction(watchOnCat);
    }
    void doSwitchPowerOn()
    {
        setAction(switchPowerOn);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("2 floor"))
            levelController.ProgrammerOn2floor = true;
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("2 floor"))
            levelController.ProgrammerOn2floor = false;
    }
}
