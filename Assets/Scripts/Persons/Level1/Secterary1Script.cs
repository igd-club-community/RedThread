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
    public PersonAct goto2level;
    public PersonAct bringPapersFrom2level;
    public PersonAct bringPapersToPrinter;
    public PersonAct bringPapersToBoss;
    public PersonAct backToDesk;
    public PersonAct prepareCoffee;
    public PersonAct talkWithCleaner;
    public PersonAct beAfraidOfProgrammer;
    public PersonAct askCleanerToBringPapers; 
    public PersonAct askCleanerToBringSugar;
    public PersonAct waitForClearFloor; //ждем пока уборщица нас пропустит
    public PersonAct sayAboutSugar;
    public PersonAct goForSugar;
    public PersonAct bringSugar;
    public PersonAct waitOnEndDialog;
    public PersonAct winDialog;

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
        levelController.EndDialogWithBoss += () => { setAction(waitOnEndDialog); };
        levelController.SecretaryWinDialog += () => { setAction(winDialog); };

        name = "secret";
        setAction(talkWithCleaner);
    }

    float timeCoffeePreparingStarted;
    float timeTalkingWithCleaner;

    new void Update()
    {
        base.Update();
        noAction = false;

        if (currentAction == talkWithCleaner)
        {
            if (levelController.CleanerIsBisy)
            {
                if (levelController.BossIsBisy && !levelController.CleanerIsBringingSugar)
                    setAction(sayAboutSugar);
                else
                    setAction(wait);
            }
        }
        else if (currentAction == wait)
        {
            if (!levelController.CleanerIsBisy)
            {
                setAction(talkWithCleaner);
            }
        }
        else if (currentAction == goto2level)
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
                        setAction(beAfraidOfProgrammer);
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
            if (levelController.BossIsBisy)
                setAction(sayAboutSugar);
            //Если уборщица занята, то есть если сломан один из кулеров или растение у босса,
            //то Мы вместо того чтобы поговорить с уборщицей ничего не делаем

            else if (levelController.CleanerIsBisy)
                doBackToDesk();
            else
                setAction(talkWithCleaner);
        }
        else if (currentAction == sayAboutSugar)
        {
            doGoForSugar();
        }
        else if (currentAction == goForSugar)
        {
            setAction(bringSugar);
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
            else
                setAction(wait);
        }
        else if (currentAction == bringPapersToBoss)
        {
            levelController.generatePapersToBossDelivered();
            doBackToDesk();
        }
        else if (currentAction == goto2level)
        {
            if (levelController.BossIsBisy)
                setAction(goForSugar);
            else
                setAction(bringPapersFrom2level);
        }
        else if (currentAction == bringPapersFrom2level)
        {
            setAction(bringPapersToPrinter);
        }
        else if (currentAction == bringPapersToPrinter)
        {
            levelController.PaperInPrinter = true;
            doPrintPapers();
        }
        else if (currentAction == beAfraidOfProgrammer)
        {
            if (levelController.BossIsBisy)
                setAction(askCleanerToBringSugar);
            else
                setAction(askCleanerToBringPapers);
        }
        else if (currentAction == askCleanerToBringPapers)
        {
            levelController.generateCleanerBringPapersEvent();
            doBackToDesk();
        }
        else if (currentAction == askCleanerToBringSugar)
        {
            levelController.generateCleanerBringSugarEvent();
            doBackToDesk();
        }
        else if (currentAction == winDialog)
        {
            levelController.win();
        }
    }

    public void doPrepareCoffee()
    {
        levelController.SecretaryIsBisy = true;
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
        levelController.SecretaryIsBisy = true;
        askedToPrintPapers = true;
        //say("Бумажки, бумажки, я несу бумажки");
        Debug.Log("doPrintPapers");

        setAction(printPapers);
    }

    public void doBringPapersFrom2level()
    {
        //say("Надо взять бумаги.");
        Debug.Log("doBringPapersFrom2level");
        setAction(goto2level);
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
        Debug.Log("goto2level");
        setAction(goto2level);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("2 floor"))
        {
            if (levelController.ProgrammerOn2floor)
            {
                setAction(beAfraidOfProgrammer);
            }
            else
            {
                levelController.SecretaryOn2floor = true;
            }
        }

        if (!levelController.GrassInBossRoomIsFine && other.CompareTag("grass"))
        {
            rememberedAction = currentAction;
            setAction(waitForClearFloor);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("2 floor"))
            levelController.SecretaryOn2floor = false;
    }
}
