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
        currentAction = actions[curActNum];
        navAgent = GetComponent<NavMeshAgent>();
        setAction(curActNum);
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(currentAction.target.position, transform.position);
        if (distance < 1)
        {
            curActNum = (curActNum + 1) % actions.Length;
        }
        setAction(curActNum);
    }

    private void setAction(int curActNum)
    {
        currentAction = actions[curActNum];
        navAgent.SetDestination(currentAction.target.position);
    }
}

public enum PersonState
{
    Idle,
    Acting
}
