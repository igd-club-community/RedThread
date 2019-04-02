using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ActingPerson : MonoBehaviour
{
    protected Animator anim;
    PersonNavigationController navigator;
    public PersonAct currentAction;
    public bool noAction = false;
    //int curActNum = 0;
    public float timeOfStart = 0; //Время начала выполнения задачи

    ActingPerson interlocutor; //Человек с которым мы общаемся
    public bool inDialog = false; //находится ли личность в диалоге
    public bool isDialogSingle = false; //проходит ли этот диалог сам с собой
    public GameObject textBackground; //фон текста
    public Text bubbleText; //текстбокс
    public int currentPhraseNum; //номер текущей фразы
    public float textMaxTime = 3; //время в течении которого отображается фраза
    public float sayTime = 0; //время начала отображения фразы


    //У персоны есть его целевая позиция куда он хочет идти
    //Есть базовый цикл из задач которые он делает пока ничего не происходит
    //У каждой отдельной персоны будет свой цикл.

    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        navigator = GetComponent<PersonNavigationController>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Здесь может установиться новое состояние, которое сразу же можно будет использовать для обновления диалога
        if (!inDialog)
        {
            if (currentAction.byTimer && Time.fixedTime - timeOfStart > currentAction.targetTimer)
            {
                goToNextAction();
            }
            else if (!currentAction.byTimer && navigator.targetReached)
            {
                goToNextAction();
            }
        }
        
        textBackground.gameObject.transform.rotation = Quaternion.identity; //new Quaternion(0,0,0,1);
        bubbleText.gameObject.transform.rotation = Quaternion.identity;

        if (Time.fixedTime - sayTime > textMaxTime)
        {
            if (currentPhraseNum >= currentAction.phrases.Length)
            {
                textBackground.SetActive(false);
                bubbleText.text = "";
            } else
            {
                //Здесь нужно начать произносить все фразы
                if (interlocutor != null) //значит будет диалог с кем-то
                {
                    if (currentPhraseNum % 2 == 1)
                        interlocutor.say(currentAction.phrases[currentPhraseNum]);
                    else
                        say(currentAction.phrases[currentPhraseNum]);
                }
                else
                {
                    //произносим фразы в монологе
                    say(currentAction.phrases[currentPhraseNum]);
                }
                currentPhraseNum += 1;
            }
        }
    }
    protected virtual void goToNextAction()
    {
        Debug.Log("Acting person next Action");
    }
    public void setAction(PersonAct newAct)
    {
        navigator.SetTarget(newAct.target, newAct.targetDistance, newAct.talkingWithPerson);

        currentPhraseNum = 0;
        if (currentAction.phrasesStorage.Length != 0)
        {
            currentAction.phraseStageNum = (currentAction.phraseStageNum + 1) % currentAction.phrasesStorage.Length;
        }
        else
            currentAction.phraseStageNum = 0;

        if (newAct.phrasesStorage.Length != 0)
            newAct.phrases = newAct.phrasesStorage[newAct.phraseStageNum].phrases;

        //Debug.Log(newAct.name);
        currentAction = newAct;
        if (currentAction.byTimer)
            timeOfStart = Time.fixedTime;

        interlocutor = newAct.target.GetComponent<ActingPerson>();
        
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