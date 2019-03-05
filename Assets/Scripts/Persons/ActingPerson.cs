using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ActingPerson : MonoBehaviour
{
    public PersonAct[] actions;
    public PersonAct currentAction;
    public bool noAction = false;
    int curActNum = 0;
    public float distance = 0;
    protected NavMeshAgent navAgent;

    public GameObject textBackground;
    public Text bubbleText;

    //У персоны есть его целевая позиция куда он хочет идти
    //Есть базовый цикл из задач которые он делает пока ничего не происходит
    //У каждой отдельной персоны будет свой цикл.

    // Start is called before the first frame update
    protected virtual void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    public float sayTime = 0;
    // Update is called once per frame
    protected virtual void Update()
    {
        if (noAction)
        {
            navAgent.enabled = false;
        }
        else
        {
            navAgent.enabled = true;
            navAgent.SetDestination(currentAction.target.position);
        }
        //Если мы долши до нужной точки
        //distance = Vector3.Distance(currentAction.target.position, transform.position);
        //if (distance < 0.1)
        //{
        //    //Тогда запускаем нужную анимацию.
        //}

        //bubbleText.text = textBackground.gameObject.transform.rotation.ToString();
        textBackground.gameObject.transform.rotation = Quaternion.identity; //new Quaternion(0,0,0,1);
        bubbleText.gameObject.transform.rotation = Quaternion.identity;

        if (Time.fixedTime - sayTime > 4)
        {
            textBackground.SetActive(false);
            bubbleText.text = "";
        }
    }

    public void setAction(PersonAct newAct)
    {
        currentAction = newAct;
        //say(newAct.phrases[UnityEngine.Random.Range(0, newAct.phrases.Length)]);
    }

    public void say(string text)
    {
        textBackground.SetActive(true);
        bubbleText.text = text;
        sayTime = Time.fixedTime;
    }
}

public enum PersonState
{
    Idle,
    Acting
}
