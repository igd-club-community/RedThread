using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss1Script : ActingPerson
{
    //public PersonAct prevAction;
    public PersonAct rememberedAction;

    public PersonAct goToBossTable; //босс идёт читать бумаги один раз, в самом начале
    public PersonAct readPapers; //Читает бумаги один раз в начале, после чего идёт просить кофе
    //public PersonAct waitSecretary;
    public PersonAct askSecretaryAboutCoffee; //просит секретаря сделать кофе, много раз по ходу цикла игры
    public PersonAct goToVault; //идет к сейфу после просьбы сделать кофе, после этого возможны два варианта, в зависимости открыт сейф или нет
    public PersonAct tryToRememberPassword; //пришел к сейфу и пытается вспомнить пароль
    public PersonAct goToBossTableToDrinkCoffee; //Подходит к столу, садится, перекидывается парой слов в секретаршей и в коцне отпускает её
    public PersonAct waitForSecretaryToLeave; //отпустив секретаршу говорим несколько слов вслед
    public PersonAct grabCup; //берет кружку в руку и говорит что-то
    public PersonAct drinkCoffee; //пара слов про то что кофе опять с сахаром, после чего идёт просить кофе снова
    public PersonAct askSecretaryAboutNewPapers; //В процессе пока босс пил кофе оно было пролито
    public PersonAct unlockShelter; //пришел к сейфу и открывает его
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

        //doGoToBossTable();
        name = "boss";
        DebugStart();
    }

    public void DebugStart()
    {
        setAction(askSecretaryAboutCoffee);
    }
    
    new void Update()
    {
        if (levelController.BossOffline)
            return;
        base.Update();
        noAction = false;
        //Если у нас растение было сломано и мы ждали что оно починится, то начинаем двигаться.
        if (noAction && levelController.GrassInBossRoomIsFine)
            noAction = false;

        if (currentAction == waitForSecretaryToLeave)
        {
        }
    }

    //задания закончившиеся естественны образом, т.е. циклы
    protected override void preFinishOfCurrentAction()
    {
        if (currentAction == askSecretaryAboutCoffee)
        {
            levelController.generateNeedCoffeEvent();
        }
        else if (currentAction == goToBossTableToDrinkCoffee)
        {
            levelController.generateSecretaryCanGoToDesk();
        }
    }

    protected override void goToNextAction()
    {
        base.goToNextAction();
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
            setAction(askSecretaryAboutCoffee);
            //rememberedAction = askSecretaryAboutCoffee;
            levelController.BossCupFilled = false;
        }
        //else if (currentAction == goToSecretary)
        //{
        //    setAction(rememberedAction);
        //}
        else if (currentAction == askSecretaryAboutCoffee)
        {
            setAction(goToVault);
        }
        else if (currentAction == goToVault)
        {
            doRememberVaultPassword();
        }
        else if (currentAction == goToBossTableToDrinkCoffee)
        {
            setAction(waitForSecretaryToLeave);
        }
        else if (currentAction == waitForSecretaryToLeave)
        {
            if (levelController.BossCupMoved)
            {
                //Если чашку подвинули, значит босс проливает кофе и идёт просить новые бумаги
                //anim.SetBool("Sit", false);
                setAction(askSecretaryAboutNewPapers);
                levelController.BossCupFilled = false;
                levelController.BossCupMoved = false;
            } else
            {
                setAction(drinkCoffee);
            }
        }
        else if (currentAction == drinkCoffee)
        {
            setAction(askSecretaryAboutCoffee);
        }
        else if (currentAction == askSecretaryAboutNewPapers)
        {
            levelController.generateNeedPapersEvent();
            setAction(unlockShelter);
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
        else if (currentAction == askSecretaryToRepairWindows)
        {
            setAction(rememberedAction);
        }
    }

    public void doGoToBossTable()
    {
        setAction(goToBossTableToDrinkCoffee);
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
        setAction(tryToRememberPassword);
    }

    public void doPigeonsGetOut()
    {
        //say("Пошли вон летучие крысы!");
        Debug.Log("doPigeonsGetOut");
        rememberedAction = currentAction;
        setAction(sayGoodbytoPigeons);
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
