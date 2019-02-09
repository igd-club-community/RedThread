using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActingPerson : MonoBehaviour
{
    public PersonAct[] actions;
    public PersonAct currentAction;
    public bool noAction = false;
    int curActNum = 0;
    public float distance = 0;
    NavMeshAgent navAgent;

    //У персоны есть его целевая позиция куда он хочет идти
    //Есть базовый цикл из задач которые он делает пока ничего не происходит
    //У каждой отдельной персоны будет свой цикл.

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //Если мы долши до нужной точки
        //distance = Vector3.Distance(currentAction.target.position, transform.position);
        //if (distance < 0.1)
        //{
        //    //Тогда запускаем нужную анимацию.
        //}

    }

    public void setAction(PersonAct newAct)
    {
        currentAction = newAct;
        navAgent.SetDestination(currentAction.target.position);

    }
}

public enum PersonState
{
    Idle,
    Acting
}
