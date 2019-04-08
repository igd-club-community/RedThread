using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Класс секретарши
public class Secterary1Script : ActingPerson
{
    //Секретарша подписывается на событие босса когда он хочет кофе.

    public PersonAct bringCoffee;
    public PersonAct waitOfBoss;
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
        levelController.PapersDelivered += doPrintPapers;
        levelController.BossNeedsToRepairWindow += doCallToRepair;
        levelController.SecretaryCanGoToDesk += doBackToDesk;

        name = "secret";
        setAction(talkWithCleaner);
    }

    float timeCoffeePreparingStarted;
    float timeTalkingWithCleaner;

    new void Update()
    {
        base.Update();
        noAction = false;

        if (currentAction == bringPapersFrom2level)
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
        }
    }

    protected override void goToNextAction()
    {
        base.goToNextAction();
        //если нас попросили напечатать бумагу, то мы идём к принтеру. Если бумаги в нём нет, то идём за ней
        if (currentAction == printPapers)
        {
            if (!levelController.PaperInPrinter)
                doBringPapersFrom2level();
            else
                setAction(bringPapersToBoss);
        }
        else if (currentAction == prepareCoffee)
        {
            doBringCoffeeToBoss();
        }
        else if (currentAction == bringCoffee)
        {
            levelController.BossCupFilled = true;
            levelController.generateCoffeeDelivered();
            setAction(waitOfBoss);
        }
        else if (currentAction == talkWithCleaner)
        {
            if (levelController.BossOffline)
                doGoForSugar();
            //Если уборщица занята, то есть если сломан один из кулеров или растение у босса,
            //то Мы вместо того чтобы поговорить с уборщицей ничего не делаем

            else if (levelController.CleanerIsBisy)
                doBackToDesk();

            setAction(talkWithCleaner);
        }
        else if (currentAction == goForSugar)
        {
            doBringSugar();
        }
        else if (currentAction == bringSugar)
        {
            doBackToDesk();
        }
        else if (currentAction == backToDesk)// && levelController.PaperInPrinter)
        {
            levelController.SecretaryIsBisy = false;
            //levelController.generateSecretaryIsBack();
            if (!levelController.CleanerIsBisy)
                doTalkWithCleaner();
        }
        else if (currentAction == bringPapersToBoss)
        {
            levelController.generatePapersToBossDelivered();
            doBackToDesk();
        }
        else if (currentAction == bringPapersFrom2level)
        {
            doBringPapersToPrinter();
        }
        else if (currentAction == bringPapersToPrinter)
        {
            levelController.PaperInPrinter = true;
            doPrintPapers();
        }
        else if (currentAction == askCleanerToBringPapers)
        {
            levelController.generateCleanerBringPapersEvent();
            doBackToDesk();
        }
    }

    public void doPrepareCoffee()
    {
        //say("Как же он вкусно пахнет!");
        Debug.Log("doPrepareCoffee");
        //Запустить анимацию приготовления кофе,
        //Animator.SetState("CookCoffee");
        setAction(prepareCoffee);
        timeCoffeePreparingStarted = Time.fixedTime;
    }
    public void doTalkWithCleaner()
    {
        //say("Как дела?");
        Debug.Log("doTalkWithCleaner");
        if (!levelController.SecretaryIsBisy)
            setAction(talkWithCleaner);
        levelController.SecretaryIsBisy = false;
        timeTalkingWithCleaner = Time.fixedTime;
    }
    public void doCallToRepair()
    {
        //say("Хорошо, вызову на завтра.");
        Debug.Log("doCallToRepair");
    }
    public void doPrintPapers()
    {
        askedToPrintPapers = true;
        //say("Бумажки, бумажки, я несу бумажки");
        Debug.Log("doPrintPapers");

        setAction(printPapers);
    }

    public void doBringPapersFrom2level()
    {
        //say("Надо взять бумаги.");
        Debug.Log("doBringPapersFrom2level");
        setAction(bringPapersFrom2level);
    }

    public void doBringPapersToPrinter()
    {
        //say("Нашла!");
        Debug.Log("doBringPapersToPrinter");

        setAction(bringPapersToPrinter);
    }

    public void doAskCleanerToBringPapers()
    {
        //say("Не-не-не, я туда сама не пойду");
        Debug.Log("doAskCleanerToBringPapers");

        setAction(askCleanerToBringPapers);
    }

    public void doBringCoffeeToBoss()
    {
        //say("Не дай бог разолью");
        Debug.Log("doBringCoffeeToBoss");
        //Принести кофе боссу
        setAction(bringCoffee);
    }

    public void doBackToDesk()
    {
        //say("Можно и отдохнуть");
        Debug.Log("doBackToDesk");
        setAction(backToDesk);
    }
    public void doGoForSugar()
    {
        levelController.SecretaryIsBisy = true;
        //say("Сахар кончился");
        Debug.Log("doGoForSugar");
        setAction(goForSugar);
    }
    public void doBringSugar()
    {
        //say("Налью себе чаю");
        Debug.Log("doBringSugar");
        setAction(bringSugar);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("2 floor"))
            levelController.SecretaryOn2floor = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!levelController.GrassInBossRoomIsFine && other.CompareTag("grass"))
        {
            noAction = true;
        }
        else
        {
            noAction = false;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("2 floor"))
            levelController.SecretaryOn2floor = false;
    }
}
