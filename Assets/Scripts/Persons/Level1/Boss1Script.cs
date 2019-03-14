using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss1Script : ActingPerson
{
    public PersonAct prevAction; 
    public PersonAct readPapers;
    public PersonAct drinkCoffee;
    public PersonAct askSecretaryAboutCoffee;
    public PersonAct goToVault;
    public PersonAct unlockShelter;
    public PersonAct askSecretaryAboutNewPapers;
    public PersonAct goToBossCabinet;
    public PersonAct goToProgrammer;
    public PersonAct talkWithProgrammer;
    public PersonAct sayGoodbytoPigeons;
    public PersonAct askSecretaryToRepairWindows;

    Level1Controller levelController;

    public bool secretaryAskedToRepairWindow = false;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        levelController = FindObjectOfType<Level1Controller>();
        levelController.CoffeeDelivered += doReadPapers;
        //levelController.PapersDelivered += doReadPapers;asdsad

        doReadPapers();
    }

    public float timeOfReadingStarted;
    public float timeOfCoffeeDelivered;
    public float timeOfTalkingWithPigeons;
    // Update is called once per frame

    new void Update()
    {
        if (levelController.BossOffline)
            return;
        base.Update();
        noAction = false;
        //Если у нас растение было сломано и мы ждали что оно починится, то начинаем двигаться.
        if (noAction && levelController.GrassInBossRoomIsFine)
            noAction = false;
        distance = Vector3.Distance(currentAction.target.position, transform.position);
        if (currentAction == drinkCoffee)
        {
            distance = Vector3.Distance(currentAction.target.position, transform.position);
            if (distance < 1 && (Time.fixedTime - timeOfCoffeeDelivered > 5))
            {
                doAskSecretaryAboutCoffee();
            }
        }
        else if (currentAction == askSecretaryAboutCoffee || currentAction == askSecretaryAboutNewPapers || currentAction == askSecretaryToRepairWindows)
        {
            if (distance < 1.5)
            {
                if (currentAction == askSecretaryAboutCoffee)
                {
                    levelController.generateNeedCoffeEvent();
                    doRememberVaultPassword();
                }
                else if (currentAction == askSecretaryAboutNewPapers)
                {
                    levelController.generateNeedPapersEvent();
                    doUnlockShelter();
                }
                else if (currentAction == askSecretaryToRepairWindows)
                {
                    setAction(prevAction);
                }
            }
        }
        else if (currentAction == unlockShelter)
        {
            if (distance < 1)
            {
                levelController.BossShelterPassIsKnown = true;
                doGoToProgrammer();
            }
        }
        else if (currentAction == readPapers)
        {
            if (Time.fixedTime - timeOfReadingStarted > 3)
            {
                if (levelController.BossCupMoved)
                {
                    //Если чашку подвинули, значит босс проливает кофе и идёт к программисту
                    doAskSecretaryAboutNewPapers();
                    levelController.BossCupFilled = false;
                    levelController.BossCupMoved = false;
                }
            }
            if (Time.fixedTime - timeOfReadingStarted > 15)
            {
                doAskSecretaryAboutCoffee();
                //levelController.generateNeedCoffeEvent();
                levelController.BossCupFilled = false;
            }
        }
        else if (currentAction == goToProgrammer && distance < 1)
        {
            if (levelController.ProgrammerDeskClear)
            {
                if (levelController.ProgrammerOn2floor)
                    doTalkWithProgrammer();
            }
            else
                doFireProgrammer();
        }
        else if (currentAction == talkWithProgrammer)
        {
            if (distance < 1.5)
            {
                noAction = true;

                //если подходим к программисту то говорим с ним до тех пор пока секретарша не распечатает бумаги
                if (levelController.PaperInBossRoom)
                    doReadPapers();
            }
        }
        else if (currentAction == sayGoodbytoPigeons && distance < 1 && Time.fixedTime - timeOfTalkingWithPigeons > 5)
        {
            if (!secretaryAskedToRepairWindow)
                doAskSecretaryToRepairWindows();
            else
                setAction(prevAction);
        }
    }

    public void doReadPapers()
    {
        say("Опять рубль падает");
        Debug.Log("doReadPapers");
        //Здесь можно запустить таймер сколько читать газеты например.
        setAction(readPapers);
        currentAction = readPapers;
        timeOfReadingStarted = Time.fixedTime;
    }

    public void doUnlockShelter()
    {
        say("Точно! Вспомнил пароль");
        Debug.Log("doUnlockShelter");

        setAction(unlockShelter);

        levelController.BossShelterPassIsKnown = true;
    }
    public void doGoToProgrammer()
    {
        say("Надо поговорить с программистом.");
        Debug.Log("doGoToProgrammer");

        setAction(goToProgrammer);

    }
    public void doTalkWithProgrammer()
    {
        say("Что ты решил?");
        Debug.Log("doTalkWithProgrammer");

        setAction(talkWithProgrammer);

    }
    public void doFireProgrammer()
    {
        say("Что за мудак");
        Debug.Log("doFireProgrammer");
        doReadPapers(); //Временно пока нет плана что будет когда увольняем

    }

    public void doAskSecretaryAboutCoffee()
    {
        say("Хочу кофе");
        Debug.Log("doAskSecretaryAboutCoffee");
        setAction(askSecretaryAboutCoffee);
    }

    public void doAskSecretaryAboutNewPapers()
    {
        say("Твоюж мать");
        Debug.Log("doAskSecretaryAboutNewPapers");
        //Запустить движение к секретарше,
        //Запустить анимацию просьбы
        setAction(askSecretaryAboutNewPapers);
    }
    public void doAskSecretaryToRepairWindows()
    {
        say("Окна теперь новые ставить");
        Debug.Log("doAskSecretaryToRepairWindows");
        secretaryAskedToRepairWindow = true;
        setAction(askSecretaryToRepairWindows);
    }

    public void doRememberVaultPassword()
    {
        say("Какой тут был пароль?");
        Debug.Log("doRememberVaultPassword");
        if (!levelController.BossShelterLocked)
            Debug.Log("BossShelter Un Locked");
        setAction(goToVault);
    }

    public void doPigeonsGetOut()
    {
        say("Пошли вон летучие крысы!");
        Debug.Log("doPigeonsGetOut");
        prevAction = currentAction;
        setAction(sayGoodbytoPigeons);
        timeOfTalkingWithPigeons = Time.fixedTime;
    }

    public void doCry()
    {
        levelController.BossOffline = true;
        say("АААааыыыыаааа!!");
        Debug.Log("doCry");
        noAction = true;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
        Debug.Log(transform.rotation);
        transform.rotation.Set(10f, 10f, 10f, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (levelController.PigeonsInBossRoom && other.CompareTag("Boss room"))
        {
            Debug.Log("Boss enter his room");
            if (levelController.BossShelterEmpty)
                doCry();
            else
                doPigeonsGetOut();
        }
        if (other.CompareTag("2 floor"))
            levelController.BossOn2floor = true;
        if (other.CompareTag("chair")) {
            Debug.Log("Time to sit");
            anim.SetBool("Sit", true);
        }
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
        if (levelController.PigeonsInBossRoom && other.CompareTag("Boss room"))
        {
            levelController.generatePigeonsInBossRoom();

        }
        if (other.CompareTag("2 floor"))
            levelController.BossOn2floor = false;
        if (other.CompareTag("chair"))
        {
            anim.SetBool("Sit", false);
        }
    }

    //public void doDrinkCoffee()
    //{
    //    Debug.Log("DrinkCoffee");
    //    //Запускаем перемещение к столу и анимацию
    //    actingPerson.setAction(drinkCoffee);
    //    currentAction = drinkCoffee;
    //    timeOfCoffeeDelivered = Time.fixedTime;
    //}
}
