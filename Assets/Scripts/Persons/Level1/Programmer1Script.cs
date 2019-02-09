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

        doWorkWirkPC();
    }

    // Update is called once per frame
    void Update()
    {
        float distance;
        if (currentAction == workWithPC)
        {
            distance = Vector3.Distance(currentAction.target.position, transform.position);
            if (distance < 1 && (Time.fixedTime - timeOfWorkingWithPC > 5))
            {
                doWatchOnDesk();
            }
        }

        if (currentAction == watchOnDesk)
        {
            distance = Vector3.Distance(currentAction.target.position, transform.position);
            if (distance < 1 && (Time.fixedTime - timeOfWatchingOnDesk > 5))
            {
                doWriteOnDesk();
            }
        }

        if (currentAction == writeOnDesk)
        {
            distance = Vector3.Distance(currentAction.target.position, transform.position);
            if (distance < 1 && (Time.fixedTime - timeOfWritingOnDesk > 5))
            {
                doWatchOnCat();
            }
        }

        if (currentAction == watchOnCat)
        {
            distance = Vector3.Distance(currentAction.target.position, transform.position);
            if (distance < 1 && (Time.fixedTime - timeOfWatchingOnCat > 5))
            {
                doWorkWirkPC();
            }
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
}
