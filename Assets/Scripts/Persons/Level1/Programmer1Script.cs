using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Programmer1Script : MonoBehaviour
{
    private ActingPerson actingPerson;
    private LevelController levelController;

    public PersonAct currentAction;
    public PersonAct workWithPC;
    public PersonAct watchOnDesk;
    public PersonAct writeOnDesk;
    public PersonAct watchOnCat;
    public PersonAct switchPowerOn;

    public float timeOfWorkingWithPC;
    public float timeOfWatchingOnDesk;
    public float timeOfWritingOnDesk;
    public float timeOfWatchingOnCat;
    // Start is called before the first frame update
    void Start()
    {
        actingPerson = GetComponent<ActingPerson>();
        levelController = FindObjectOfType<LevelController>();
        //levelController.BossNeedsCoffee += doWorkWirkPC;
        //levelController.CoffeeDelivered += doBackToDesk;
        levelController.PowerOff += doSwitchPowerOn;

        doWorkWirkPC();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(currentAction.target.position, transform.position);
        if (currentAction == workWithPC)
        {
            if (distance < 1 && (Time.fixedTime - timeOfWorkingWithPC > 5))
            {
                doWatchOnDesk();
            }
        } else 

        if (currentAction == watchOnDesk)
        {
            if (distance < 1 && (Time.fixedTime - timeOfWatchingOnDesk > 5))
            {
                doWriteOnDesk();
            }
        } else 

        if (currentAction == writeOnDesk)
        {
            if (distance < 1 && (Time.fixedTime - timeOfWritingOnDesk > 5))
            {
                doWatchOnCat();
            }
        } else 

        if (currentAction == watchOnCat)
        {
            if (distance < 1 && (Time.fixedTime - timeOfWatchingOnCat > 5))
            {
                doWorkWirkPC();
            }
        } else if (currentAction == switchPowerOn & distance <2)
        {
            levelController.generatePowerOn();
            doWorkWirkPC();
        }

    }

    void doWorkWirkPC()
    {
        actingPerson.setAction(workWithPC);
        currentAction = workWithPC;
        timeOfWorkingWithPC = Time.fixedTime;
    }
    void doWatchOnDesk()
    {
        actingPerson.setAction(watchOnDesk);
        currentAction = watchOnDesk;
        timeOfWatchingOnDesk = Time.fixedTime;
    }
    void doWriteOnDesk()
    {
        actingPerson.setAction(writeOnDesk);
        currentAction = writeOnDesk;
        timeOfWritingOnDesk = Time.fixedTime;
    }
    void doWatchOnCat()
    {
        actingPerson.setAction(watchOnCat);
        currentAction = watchOnCat;
        timeOfWatchingOnCat = Time.fixedTime;
    }
    void doSwitchPowerOn()
    {
        actingPerson.setAction(switchPowerOn);
        currentAction = switchPowerOn;
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
