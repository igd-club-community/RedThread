using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Класс секретарши
public class Secterary1Script : MonoBehaviour
{
    //Секретарша подписывается на событие босса когда он хочет кофе.

    public PersonAct currentAction;
    public PersonAct bringCoffee;
    public PersonAct printPapers;
    public PersonAct bringPapersFrom2level;
    public PersonAct bringPapersToPrinter;
    public PersonAct bringPapersToBoss;
    public PersonAct backToDesk;
    public PersonAct prepareCoffee;
    public PersonAct talkWithCleaner;
    public PersonAct askCleanerToBringPapers;
    public float distance;
    public Transform visionPoint;
    public Transform programmer;

    private ActingPerson actingPerson;
    private LevelController levelController;
    Level1States states;

    void Start()
    {
        actingPerson = GetComponent<ActingPerson>();
        levelController = FindObjectOfType<LevelController>();
        levelController.BossNeedsCoffee += doPrepareCoffee;
        levelController.BossNeedsPapers += doPrintPapers;
        levelController.CleanerDeliverPapers += doPrintPapers;
        //levelController.CoffeeDelivered += doBackToDesk;

        states = FindObjectOfType<Level1States>();

        actingPerson.setAction(talkWithCleaner);
        currentAction = talkWithCleaner;
    }

    float timeCoffeePreparingStarted;
    void Update()
    {
        distance = Vector3.Distance(currentAction.target.position, transform.position);
        if (currentAction == talkWithCleaner)
        {
            if (distance < 2)
            {
                //Стоять на месте и говорить с уборщицей.
            }
            if (distance > 2)
            {
                //идти к уборщице
            }
        }
        else if (currentAction == prepareCoffee)
        {
            if (distance < 1 && (Time.fixedTime - timeCoffeePreparingStarted > 5))
            {
                doBringCoffeeToBoss();
            }
        }
        else if (currentAction == bringCoffee && distance < 1)
        {
            states.BossCupFilled = true;
            levelController.generateCoffeeDelivered();
            doBackToDesk();
        }
        else if (currentAction == backToDesk && distance < 1 && states.PaperInPrinter)
        {
            levelController.generateSecretaryIsBack();
            actingPerson.setAction(talkWithCleaner);
            currentAction = talkWithCleaner;
        }
        else if (currentAction == printPapers && distance < 1)
        {
            if (states.PaperInPrinter)
            {
                actingPerson.setAction(bringPapersToBoss);
                currentAction = bringPapersToBoss;
            }
            else
            {
                actingPerson.setAction(bringPapersFrom2level);
                currentAction = bringPapersFrom2level;
            }
        }
        else if (currentAction == bringPapersToBoss && distance < 1)
        {
            doBackToDesk();
        }
        else if (currentAction == bringPapersFrom2level)
        {
            RaycastHit hit;
            Vector3 direction = programmer.position - visionPoint.position;
            if (Physics.Raycast(visionPoint.position, direction, out hit))
            {
                //Debug.Log("HIT " + hit.collider.tag);
                if (hit.collider.CompareTag("Person"))
                {
                    if(hit.collider.gameObject.GetComponentInParent<Programmer1Script>() != null)
                    {
                        doAskCleanerToBringPapers();
                    }
                }
            }
            if (distance < 1)
                doBringPapersToPrinter();
        }
        else if (currentAction == bringPapersToPrinter && distance < 1)
        {
            states.PaperInPrinter = true;
            doPrintPapers();
        }
        else if (currentAction == askCleanerToBringPapers && distance < 1.5)
        {
            levelController.generateCleanerBringPapersEvent();
            doBackToDesk();
        }
    }

    public void doPrepareCoffee()
    {
        actingPerson.say("Как же он вкусно пахнет!");
        Debug.Log("doPrepareCoffee");
        //Запустить анимацию приготовления кофе,
        //Animator.SetState("CookCoffee");
        actingPerson.setAction(prepareCoffee);
        currentAction = prepareCoffee;
        timeCoffeePreparingStarted = Time.fixedTime;
    }

    public void doPrintPapers()
    {
        actingPerson.say("Бумажки, бумажки, я несу бумажки");
        Debug.Log("doPrintPapers");

        actingPerson.setAction(printPapers);
        currentAction = printPapers;
    }

    public void doBringPapersToPrinter()
    {
        actingPerson.say("Нашла!");
        Debug.Log("doBringPapersToPrinter");

        actingPerson.setAction(bringPapersToPrinter);
        currentAction = bringPapersToPrinter;
    }
    public void doAskCleanerToBringPapers()
    {
        actingPerson.say("Не-не-не, я туда сама не пойду");
        Debug.Log("doAskCleanerToBringPapers");

        actingPerson.setAction(askCleanerToBringPapers);
        currentAction = askCleanerToBringPapers;
    }

    public void doBringCoffeeToBoss()
    {
        actingPerson.say("Не дай бог разолью");
        Debug.Log("doBringCoffeeToBoss");
        //Принести кофе боссу
        actingPerson.setAction(bringCoffee);
        currentAction = bringCoffee;
    }

    public void doBackToDesk()
    {
        actingPerson.say("Можно и отдохнуть");
        Debug.Log("doBackToDesk");
        actingPerson.setAction(backToDesk);
        currentAction = backToDesk;
    }

}
