using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss1Script : ActingPerson
{
    public PersonAct goToBossTable; //босс идёт читать бумаги один раз, в самом начале
    public PersonAct readPapers; //Читает бумаги один раз в начале, после чего идёт просить кофе
    public PersonAct askSecretaryAboutCoffee; //просит секретаря сделать кофе, много раз по ходу цикла игры
    public PersonAct goToVault; //идет к сейфу после просьбы сделать кофе, после этого возможны два варианта, в зависимости открыт сейф или нет
    public PersonAct tryToRememberPassword; //пришел к сейфу и пытается вспомнить пароль
    public PersonAct goToBossTableToDrinkCoffee; //Подходит к столу, садится, перекидывается парой слов в секретаршей и в коцне отпускает её
    public PersonAct waitForSecretaryToLeave; //отпустив секретаршу говорим несколько слов вслед
    public PersonAct grabCup; //берет кружку в руку и говорит что-то
    public PersonAct drinkCoffee; //пара слов про то что кофе опять с сахаром, после чего идёт просить кофе снова
    public PersonAct spillСoffee; //В процессе пока босс пил кофе оно было пролито
    public PersonAct askSecretaryAboutNewPapers; //просим новые бумаги если пролили кофе
    public PersonAct unlockShelter; //пришел к сейфу и проверяет пароль
    public PersonAct openShelter; //пришел к сейфу и открывает его
    public PersonAct closeShelter; //пришел к сейфу и видит что он открыт
    public PersonAct cryNearShelter; //пришел к сейфу и видит что документы украдены
    public PersonAct waitForClearFloor; //ждем пока уборщица нас пропустит
    public PersonAct goToProgrammer;
    public PersonAct talkWithProgrammer;
    public PersonAct sayGoodbytoPigeons;
    public PersonAct askSecretaryToRepairWindows;

    Level1Controller levelController;

    public bool wantCoffee = false;
    public bool wantPapers = false;
    public bool secretaryAskedToRepairWindow = false;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        levelController = FindObjectOfType<Level1Controller>();
        levelController.CoffeeDelivered += doGoToBossTableToDrinkCoffee;
        levelController.PasswordRemembered += doUnlockShelter;

        setAction(readPapers);

        name = "boss";
        //DebugStart();
    }

    public void DebugStart()
    {
        setAction(askSecretaryAboutCoffee);
    }
    
    new void Update()
    {
        base.Update();
        noAction = false;
        //Если у нас растение было сломано и мы ждали что оно починится, то начинаем двигаться.
        //if (noAction && levelController.GrassInBossRoomIsFine)
        //    noAction = false;

        if (currentAction == goToProgrammer)
        {
            if (levelController.ProgrammerOn2floor)
                doTalkWithProgrammer();
        }
        if (currentAction == waitForClearFloor && levelController.GrassInBossRoomIsFine)
        {
            setAction(rememberedAction);
            levelController.BossIsBisy = false;
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
        if (currentAction == readPapers)
        {
            setAction(askSecretaryAboutCoffee);
            levelController.BossCupFilled = false;
        }
        //else if (currentAction == goToBossTable)
        //{
        //    //anim.SetBool("Sit", true);
        //    Debug.Log("BossScript readPapers");
        //    setAction(readPapers);
        //}
        else if (currentAction == askSecretaryAboutCoffee)
        {
            setAction(goToVault);
        }
        else if (currentAction == goToVault)
        {
            doRememberVaultPassword(); //Идем вспоминать пароль пока секретарша не принесет кофе
        }
        else if (currentAction == goToBossTableToDrinkCoffee)
        {
            //anim.SetBool("Sit", true);
            setAction(waitForSecretaryToLeave);
        }
        else if (currentAction == waitForSecretaryToLeave)
        {
            if (levelController.BossCupMoved)
            {
                //Если чашку подвинули, значит босс проливает кофе и идёт просить новые бумаги
                setAction(spillСoffee);
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
        else if (currentAction == spillСoffee)
        {
            setAction(askSecretaryAboutNewPapers);
        }
        else if (currentAction == askSecretaryAboutNewPapers)
        {
            levelController.generateNeedPapersEvent(); //запускает печать бумаги секретаршей и подзывает уборщицу
            if (levelController.passwordRemembered)
                setAction(goToVault);
            else
                setAction(wait);
        }
        else if (currentAction == goToVault)
        {
            if (!levelController.BossShelterLocked)
                setAction(closeShelter);
            else if (levelController.passwordRemembered)
                setAction(openShelter);
            else if (levelController.documentsStolen)
            {
                setAction(cryNearShelter);
                levelController.BossIsBisy = true;
            }
        }
        else if (currentAction == unlockShelter || currentAction == openShelter || currentAction == closeShelter)
        {
            levelController.BossShelterPassIsKnown = true;
            setAction(goToProgrammer);
        }
        else if (currentAction == goToProgrammer)
        {
            if (levelController.ProgrammerDeskClear)
            {
                //здесь мы просто ждём, пока программист вернётся и проверяем это в апдейте
            }
            else
                doFireProgrammer();
        }
        else if (currentAction == talkWithProgrammer)
        {
            talkWithProgrammer.target.GetComponent<ActingPerson>().releaseFromAnswer();
            setAction(readPapers);
            //если подходим к программисту то говорим с ним до тех пор пока секретарша не распечатает бумаги
            //if (levelController.PaperInBossRoom)
            //    doGoToBossTableToDrinkCoffee();
            // нет, будем говорить пока диалог не закончится

        }
        else if (currentAction == sayGoodbytoPigeons)
        {
            levelController.pigeons.SetActive(false);
            if (levelController.documentsStolen)
                setAction(cryNearShelter);
            else if (!secretaryAskedToRepairWindow && !levelController.SecretaryIsBisy)
                doAskSecretaryToRepairWindows();
            else
                setAction(rememberedAction);
        }
        else if (currentAction == askSecretaryToRepairWindows)
        {
            setAction(rememberedAction);
        }
    }

    public void doGoToBossTableToDrinkCoffee()
    {
        setAction(goToBossTableToDrinkCoffee);
    }
    public void doUnlockShelter()
    {
        setAction(unlockShelter);
    }
    public void doTalkWithProgrammer()
    {
        talkWithProgrammer.target.GetComponent<ActingPerson>().forceToAnswer();
        setAction(talkWithProgrammer);
    }
    public void doFireProgrammer()
    {
        //say("Что за мудак");
        Debug.Log("doFireProgrammer");
        doGoToBossTableToDrinkCoffee(); //Временно пока нет плана что будет когда увольняем

    }
    
    public void doAskSecretaryToRepairWindows()
    {
        //say("Окна теперь новые ставить");
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
        levelController.BossIsBisy = true;
        setAction(cryNearShelter);
        //say("АААааыыыыаааа!!");
        //Debug.Log("doCry");
        //noAction = true;
        //GetComponent<NavMeshAgent>().enabled = false;
        //GetComponent<Rigidbody>().isKinematic = false;
        //Debug.Log(transform.rotation);
        //transform.rotation.Set(10f, 10f, 10f, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (levelController.PigeonsInBossRoom && other.CompareTag("Boss room"))
        {
            Debug.Log("Boss enter his room");
            if (levelController.documentsStolen)
                doCry();
            else
                doPigeonsGetOut();
        }
        if (other.CompareTag("2 floor"))
            levelController.BossOn2floor = true;
        if (other.CompareTag("Boss room"))
            levelController.BossInBossRoom = true;

        if (!levelController.GrassInBossRoomIsFine && other.CompareTag("grass"))
        {
            rememberedAction = currentAction;
            setAction(waitForClearFloor);
            levelController.BossIsBisy = true;
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
        if (other.CompareTag("Boss room"))
            levelController.BossInBossRoom = false;
    }
}
