using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss1Script : ActingPerson
{
    //public PersonAct prevAction;
    public PersonAct rememberedAction;
    public PersonAct goToBossTable;
    public PersonAct readPapers;
    public PersonAct goToSecretary;
    public PersonAct askSecretaryAboutCoffee;
    public PersonAct goToVault;
    public PersonAct tryToRememberPassword;
    public PersonAct unlockShelter;
    public PersonAct askSecretaryAboutNewPapers;
    public PersonAct goToProgrammer;
    public PersonAct talkWithProgrammer;
    public PersonAct sayGoodbytoPigeons;
    public PersonAct askSecretaryToRepairWindows;

    Level1Controller levelController;

    public bool wantCoffee = false;
    public bool wantPapers = false;
    public bool passwordRemembered = false;
    public bool secretaryAskedToRepairWindow = false;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        levelController = FindObjectOfType<Level1Controller>();
        levelController.CoffeeDelivered += doGoToBossTable;
        //levelController.PapersDelivered += doReadPapers;

        doGoToBossTable();
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

        if (currentAction == readPapers)
        {
            if (levelController.BossCupMoved)
            {
                //Если чашку подвинули, значит босс проливает кофе и идёт просить новые бумаги
                anim.SetBool("Sit", false);

                say("Твоюж мать!");
                setAction(goToSecretary);
                rememberedAction = askSecretaryAboutNewPapers;

                levelController.BossCupFilled = false;
                levelController.BossCupMoved = false;
            }
        }
    }

    //задания закончившиеся естественны образом, т.е. циклы
    protected override void goToNextAction()
    {
        Debug.Log("BossScript next Action");
        if (currentAction == goToBossTable)
        {
            anim.SetBool("Sit", true);
            Debug.Log("BossScript readPapers");
            setAction(readPapers);
        }
        else if (currentAction == readPapers)
        {
            anim.SetBool("Sit", false);
            setAction(goToSecretary);
            rememberedAction = askSecretaryAboutCoffee;
            //doAskSecretaryAboutCoffee();
            //levelController.generateNeedCoffeEvent();
            levelController.BossCupFilled = false;
        }
        else if (currentAction == goToSecretary)
        {
            setAction(rememberedAction);
        }
        else if (currentAction == askSecretaryAboutCoffee)
        {
            levelController.generateNeedCoffeEvent();
            doRememberVaultPassword();
        }
        else if (currentAction == askSecretaryAboutNewPapers)
        {
            levelController.generateNeedPapersEvent();
            setAction(unlockShelter);
        }
        else if (currentAction == askSecretaryToRepairWindows)
        {
            setAction(rememberedAction);
        }
        else if (currentAction == goToVault)
        {
            setAction(tryToRememberPassword);
        }
        else if (currentAction == unlockShelter)
        {
            levelController.BossShelterPassIsKnown = true;
            setAction(goToProgrammer);
        }
        else if (currentAction == goToProgrammer)
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
            noAction = true;

            //если подходим к программисту то говорим с ним до тех пор пока секретарша не распечатает бумаги
            if (levelController.PaperInBossRoom)
                doGoToBossTable();

        }
        else if (currentAction == sayGoodbytoPigeons)
        {
            if (!secretaryAskedToRepairWindow)
                doAskSecretaryToRepairWindows();
            else
                setAction(rememberedAction);
        }
    }

    public void doGoToBossTable()
    {
        setAction(goToBossTable);
    }
    public void doTalkWithProgrammer()
    {
        setAction(talkWithProgrammer);

    }
    public void doFireProgrammer()
    {
        say("Что за мудак");
        Debug.Log("doFireProgrammer");
        doGoToBossTable(); //Временно пока нет плана что будет когда увольняем

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
        //say("Какой тут был пароль?");
        Debug.Log("doRememberVaultPassword");
        if (!levelController.BossShelterLocked)
            Debug.Log("BossShelter Un Locked");
        setAction(goToVault);
    }

    public void doPigeonsGetOut()
    {
        //say("Пошли вон летучие крысы!");
        Debug.Log("doPigeonsGetOut");
        rememberedAction = currentAction;
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
        //if (other.CompareTag("chair"))
        //{
        //    Debug.Log("Time to sit");
        //    anim.SetBool("Sit", true);
        //}
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
        //if (other.CompareTag("chair"))
        //{
        //    anim.SetBool("Sit", false);
        //}
    }
}
