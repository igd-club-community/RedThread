using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss1Script : MonoBehaviour
{
    public PersonAct currentAction;
    public PersonAct readPapers;
    public PersonAct drinkCoffee;
    public PersonAct askSecretaryAboutCoffee;
    public PersonAct goToVault;
    public PersonAct unlockShelter;
    public PersonAct askSecretaryAboutNewPapers;
    public PersonAct goToBossCabinet;
    public PersonAct talkWithProgrammer;
    public PersonAct sayGoodbytoPigeons;
    public PersonAct askSecretaryToRepairWindows;


    private ActingPerson actingPerson;
    LevelController levelController;
    Level1States states;
    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        actingPerson = GetComponent<ActingPerson>();
        levelController = FindObjectOfType<LevelController>();
        levelController.CoffeeDelivered += doReadPapers;

        states = FindObjectOfType<Level1States>();

        doReadPapers();
    }

    public float timeOfReadingStarted;
    public float timeOfCoffeeDelivered;
    public float timeOfTalkingWithPigeons;
    // Update is called once per frame
    void Update()
    {
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
                } else if (currentAction == askSecretaryToRepairWindows)
                {
                    doReadPapers();
                }
            }
        }
        else if (currentAction == unlockShelter)
        {
            if (distance < 1)
            {
                states.BossShelterPassIsKnown = true;
                doTalkWithProgrammer();
            }
        }
        else if (currentAction == readPapers)
        {
            if (Time.fixedTime - timeOfReadingStarted > 3)
            {
                if (states.BossCupMoved)
                {
                    //Если чашку подвинули, значит босс проливает кофе и идёт к программисту
                    doAskSecretaryAboutNewPapers();
                    states.BossCupFilled = false;
                    states.BossCupMoved = false;
                }
            }
            if (Time.fixedTime - timeOfReadingStarted > 5)
            {
                doAskSecretaryAboutCoffee();
                //levelController.generateNeedCoffeEvent();
                states.BossCupFilled = false;
            }
        }
        else if (currentAction == talkWithProgrammer && distance < 1)
        {
            if (states.ProgrammerDeskClear)
                doReadPapers();
            else
                doFireProgrammer();
        } else if (currentAction == sayGoodbytoPigeons && distance < 1 && Time.fixedTime - timeOfTalkingWithPigeons > 10)
        {
            doAskSecretaryToRepairWindows();
        }
    }

    public void doReadPapers()
    {
        actingPerson.say("Опять рубль падает");
        Debug.Log("doReadPapers");
        //Здесь можно запустить таймер сколько читать газеты например.
        actingPerson.setAction(readPapers);
        currentAction = readPapers;
        timeOfReadingStarted = Time.fixedTime;
    }

    public void doUnlockShelter()
    {
        actingPerson.say("Точно! Вспомнил пароль");
        Debug.Log("doUnlockShelter");
        
        actingPerson.setAction(unlockShelter);
        currentAction = unlockShelter;

        states.BossShelterPassIsKnown = true;
    }
    public void doTalkWithProgrammer()
    {
        actingPerson.say("Надо поговорить с программистом.");
        Debug.Log("doTalkWithProgrammer");

        actingPerson.setAction(talkWithProgrammer);
        currentAction = talkWithProgrammer;

    }
    public void doFireProgrammer()
    {
        actingPerson.say("Что за мудак");
        Debug.Log("doFireProgrammer");
        doReadPapers(); //Временно пока нет плана что будет когда увольняем

    }

    public void doAskSecretaryAboutCoffee()
    {
        actingPerson.say("Хочу кофе");
        Debug.Log("doAskSecretaryAboutCoffee");
        actingPerson.setAction(askSecretaryAboutCoffee);
        currentAction = askSecretaryAboutCoffee;
    }

    public void doAskSecretaryAboutNewPapers()
    {
        actingPerson.say("Твоюж мать");
        Debug.Log("doAskSecretaryAboutNewPapers");
        //Запустить движение к секретарше,
        //Запустить анимацию просьбы
        actingPerson.setAction(askSecretaryAboutNewPapers);
        currentAction = askSecretaryAboutNewPapers;
    }
    public void doAskSecretaryToRepairWindows()
    {
        actingPerson.say("Окна теперь новые ставить");
        Debug.Log("doAskSecretaryToRepairWindows");

        actingPerson.setAction(askSecretaryToRepairWindows);
        currentAction = askSecretaryToRepairWindows;
    }

    public void doRememberVaultPassword()
    {
        actingPerson.say("Какой тут был пароль?");
        Debug.Log("doRememberVaultPassword");
        if (!states.BossShelterLocked)
            Debug.Log("BossShelter Un Locked");
        actingPerson.setAction(goToVault);
        currentAction = goToVault;
    }

    public void doPigeonsGetOut()
    {
        actingPerson.say("Пошли вон летучие крысы!");
        Debug.Log("doPigeonsGetOut");
        actingPerson.setAction(sayGoodbytoPigeons);
        currentAction = sayGoodbytoPigeons;
        timeOfTalkingWithPigeons = Time.fixedTime;
    }
    public void doCry()
    {
        actingPerson.say("АААааыыыыаааа!!");
        Debug.Log("doCry");
        actingPerson.noAction = true;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
        Debug.Log(transform.rotation);
        transform.rotation.Set(10f, 10f, 10f, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (states.PigeonsInBossRoom && other.CompareTag("Boss room"))
        {
            Debug.Log("Boss enter his room");
            if (states.BossShelterEmpty)
                doCry();
            else
                doPigeonsGetOut();
        }
        if (other.CompareTag("2 floor"))
            levelController.BossOn2floor = true;
    }

    public void OnTriggerExit(Collider other)
    {
        if (states.PigeonsInBossRoom && other.CompareTag("Boss room"))
        {
            levelController.generatePigeonsInBossRoom();
        }
        if (other.CompareTag("2 floor"))
            levelController.BossOn2floor = false;
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
