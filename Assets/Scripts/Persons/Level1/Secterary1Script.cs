﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Класс секретарши
public class Secterary1Script : ActingPerson
{
    //Секретарша подписывается на событие босса когда он хочет кофе.

    public PersonAct bringCoffee;
    public PersonAct printPapers;
    public PersonAct bringPapersFrom2level;
    public PersonAct bringPapersToPrinter;
    public PersonAct bringPapersToBoss;
    public PersonAct backToDesk;
    public PersonAct prepareCoffee;
    public PersonAct talkWithCleaner;
    public PersonAct askCleanerToBringPapers;
    public PersonAct goForSugar;
    public PersonAct bringSugar;

    public Transform visionPoint;
    public Transform programmer;

    private Level1Controller levelController;

    public bool askedToPrintPapers = false;
    public bool CleanerAskedToBringPapers = false;

    new void Start()
    {
        base.Start();
        levelController = FindObjectOfType<Level1Controller>();
        levelController.BossNeedsCoffee += doPrepareCoffee;
        levelController.BossNeedsPapers += doPrintPapers;
        levelController.CleanerBringPapers += doPrintPapers;
        levelController.BossNeedsToRepairWindow += doCallToRepair;


        setAction(talkWithCleaner);
    }

    float timeCoffeePreparingStarted;
    float timeTalkingWithCleaner;

    new void Update()
    {
        base.Update();
        noAction = false;
        distance = Vector3.Distance(currentAction.target.position, transform.position);

        //если нас попросили напечатать бумагу, то мы идём к принтеру. Если бумаги в нём нет, то идём за ней
        if (currentAction == printPapers && distance < 1)
        {
            if (levelController.PaperInPrinter)
                doBringPapersFrom2level();
            else
                setAction(bringPapersToBoss);
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
            levelController.BossCupFilled = true;
            levelController.generateCoffeeDelivered();
            doBackToDesk();
        }
        else if (currentAction == talkWithCleaner)
        {
            if (levelController.BossOffline && Time.fixedTime - timeTalkingWithCleaner > 10)
                doGoForSugar();
            //Если уборщица занята, то есть если сломан один из кулеров или растение у босса,
            //то Мы вместо того чтобы поговорить с уборщицей ничего не делаем


            if (distance > 2)
            {
                setAction(talkWithCleaner);
                //идти к уборщице
            }
            else
            {
                noAction = true;
                //Стоять на месте и говорить с уборщицей.
            }
        }
        else if (currentAction == goForSugar && distance < 1)
        {
            doBringSugar();
        }
        else if (currentAction == bringSugar && distance < 1)
        {
            doBackToDesk();
        }
        else if (currentAction == backToDesk && distance < 1)// && levelController.PaperInPrinter)
        {
            levelController.generateSecretaryIsBack();
            doTalkWithCleaner();
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
                    if (hit.collider.gameObject.GetComponentInParent<Programmer1Script>() != null)
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
            levelController.PaperInPrinter = true;
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
        say("Как же он вкусно пахнет!");
        Debug.Log("doPrepareCoffee");
        //Запустить анимацию приготовления кофе,
        //Animator.SetState("CookCoffee");
        setAction(prepareCoffee);
        timeCoffeePreparingStarted = Time.fixedTime;
    }
    public void doTalkWithCleaner()
    {
        say("Как дела?");
        Debug.Log("doTalkWithCleaner");
        if (!levelController.SecretaryIsBisy)
            setAction(talkWithCleaner);
        levelController.SecretaryIsBisy = false;
        timeTalkingWithCleaner = Time.fixedTime;
    }
    public void doCallToRepair()
    {
        say("Хорошо, вызову на завтра.");
        Debug.Log("doCallToRepair");
    }
    public void doPrintPapers()
    {
        askedToPrintPapers = true;
        say("Бумажки, бумажки, я несу бумажки");
        Debug.Log("doPrintPapers");

        setAction(printPapers);
    }

    public void doBringPapersFrom2level()
    {
        say("Надо взять бумаги.");
        Debug.Log("doBringPapersFrom2level");
        setAction(bringPapersFrom2level);
    }

    public void doBringPapersToPrinter()
    {
        say("Нашла!");
        Debug.Log("doBringPapersToPrinter");

        setAction(bringPapersToPrinter);
    }

    public void doAskCleanerToBringPapers()
    {
        say("Не-не-не, я туда сама не пойду");
        Debug.Log("doAskCleanerToBringPapers");

        setAction(askCleanerToBringPapers);
    }

    public void doBringCoffeeToBoss()
    {
        say("Не дай бог разолью");
        Debug.Log("doBringCoffeeToBoss");
        //Принести кофе боссу
        setAction(bringCoffee);
    }

    public void doBackToDesk()
    {
        say("Можно и отдохнуть");
        Debug.Log("doBackToDesk");
        setAction(backToDesk);
    }
    public void doGoForSugar()
    {
        levelController.SecretaryIsBisy = true;
        say("Сахар кончился");
        Debug.Log("doGoForSugar");
        setAction(goForSugar);
    }
    public void doBringSugar()
    {
        say("Налью себе чаю");
        Debug.Log("doBringSugar");
        setAction(bringSugar);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("2 floor"))
            levelController.SecretaryOn2floor = true;
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("2 floor"))
            levelController.SecretaryOn2floor = false;
    }
}