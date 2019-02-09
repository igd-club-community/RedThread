using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Класс секретарши
public class Secterary1Script : MonoBehaviour
{
    //Секретарша подписывается на событие босса когда он хочет кофе.

    private ActingPerson actingPerson;
    private LevelController levelController;

    public PersonAct currentAction;
    public PersonAct bringCoffee;
    public PersonAct backToDesk;
    public PersonAct prepareCoffee;
    public PersonAct talkWithCleaner;

    void Start()
    {
        actingPerson = GetComponent<ActingPerson>();
        levelController = FindObjectOfType<LevelController>();
        levelController.BossNeedsCoffee += doPrepareCoffee;
        //levelController.CoffeeDelivered += doBackToDesk;

        actingPerson.setAction(talkWithCleaner);
        currentAction = talkWithCleaner;
    }

    float timeCoffeePreparingStarted;
    void Update()
    {
        float distance;
        if (currentAction == talkWithCleaner)
        {
            distance = Vector3.Distance(currentAction.target.position, transform.position);
            if (distance < 2)
            {
                //Стоять на месте и говорить с уборщицей.
            }
            if (distance > 2)
            {
                //идти к уборщице
            }
        }
        if (currentAction == prepareCoffee)
        {
            distance = Vector3.Distance(currentAction.target.position, transform.position);
            if (distance < 1 && (Time.fixedTime - timeCoffeePreparingStarted > 5))
            {
                doBringCoffeeToBoss();
            }
        }

        if (currentAction == bringCoffee)
        {
            distance = Vector3.Distance(currentAction.target.position, transform.position);
            if (distance < 1)
            {
                levelController.generateCoffeeDelivered();
                doBackToDesk();
            }
        }
        if (currentAction == backToDesk)
        {
            distance = Vector3.Distance(currentAction.target.position, transform.position);
            if (distance < 1)
            {
                levelController.generateSecretaryIsBack();
                actingPerson.setAction(talkWithCleaner);
            }
        }
    }

    public void doPrepareCoffee()
    {
        Debug.Log("doPrepareCoffee");
        //Запустить анимацию приготовления кофе,
        //Animator.SetState("CookCoffee");
        actingPerson.setAction(prepareCoffee);
        currentAction = prepareCoffee;
        timeCoffeePreparingStarted = Time.fixedTime;
    }
    public void doBringCoffeeToBoss()
    {
        Debug.Log("doBringCoffeeToBoss");
        //Принести кофе боссу
        actingPerson.setAction(bringCoffee);
        currentAction = bringCoffee;
    }

    public void doBackToDesk()
    {
        Debug.Log("doBackToDesk");
        actingPerson.setAction(backToDesk);
        currentAction = backToDesk;
    }

}
