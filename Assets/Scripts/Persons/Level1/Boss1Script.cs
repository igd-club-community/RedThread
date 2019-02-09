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

    private ActingPerson actingPerson;
    LevelController levelController;
    // Start is called before the first frame update
    void Start()
    {
        actingPerson = GetComponent<ActingPerson>();
        levelController = FindObjectOfType<LevelController>();
        levelController.CoffeeDelivered += doDrinkCoffee;

        doReadPapers();
    }

    public float timeOfReadingStarted;
    public float timeOfCoffeeDelivered;
    // Update is called once per frame
    void Update()
    {
        float distance;
        if (currentAction == drinkCoffee)
        {
            distance = Vector3.Distance(currentAction.target.position, transform.position);
            if (distance < 1 && (Time.fixedTime - timeOfCoffeeDelivered > 5))
            {
                doAskSecretaryAboutCoffee();
            }
        }
        if (currentAction == askSecretaryAboutCoffee)
        {
            distance = Vector3.Distance(currentAction.target.position, transform.position);
            if (distance < 1)
            {
                doRememberVaultPassword();
                levelController.generateNeedCoffeEvent();
            }
        }
        if (currentAction == readPapers)
        {
            distance = Vector3.Distance(currentAction.target.position, transform.position);
            if (Time.fixedTime - timeOfReadingStarted > 5)
            {
                doAskSecretaryAboutCoffee();
                //levelController.generateNeedCoffeEvent();
            }
        }
    }

    public void doReadPapers()
    {
        Debug.Log("doReadPapers");
        //Здесь можно запустить таймер сколько читать газеты например.
        actingPerson.setAction(readPapers);
        currentAction = readPapers;
        timeOfReadingStarted = Time.fixedTime;
    }

    public void doAskSecretaryAboutCoffee()
    {
        Debug.Log("askSecretaryAboutCoffee");
        //Запустить движение к секретарше,
        //Запустить анимацию просьбы
        actingPerson.setAction(askSecretaryAboutCoffee);
        currentAction = askSecretaryAboutCoffee;
    }

    public void doRememberVaultPassword()
    {
        Debug.Log("doRememberVaultPassword");
        actingPerson.setAction(goToVault);
        currentAction = goToVault;
    }

    public void doDrinkCoffee()
    {
        Debug.Log("DrinkCoffee");
        //Запускаем перемещение к столу и анимацию
        actingPerson.setAction(drinkCoffee);
        currentAction = drinkCoffee;
        timeOfCoffeeDelivered = Time.fixedTime;
    }
}
