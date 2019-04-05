using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ActingPerson : MonoBehaviour
{
    public string name;
    protected Animator anim;
    PersonNavigationController navigator;
    public PersonAct currentAction;
    public PersonState state;
    public bool noAction = false;
    //int curActNum = 0;
    public float timeOfStart = 0; //Время начала выполнения задачи

    public bool isDialogPossible = false;
    public bool inDialog = false; //находится ли личность в диалоге
    public bool isDialogFinished = false; //проходит ли этот диалог сам с собой
    public GameObject textBackground; //фон текста
    public Text bubbleText; //текстбокс
    public Dialog currentDialog;
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
        state = PersonState.ReadyForDialog;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        switch (state)
        {
            case PersonState.ReadyForDialog:
                //Мы сначала проверяем можем ли мы войти в диалог
                if (navigator.targetReached)
                    state = PersonState.InDialog;
                break;

            case PersonState.InDialog:
                //Если с момента сказания фразы прошло больше textMaxTime секунд
                if (Time.fixedTime - sayTime > textMaxTime)
                {
                    //Тогда мы переходим к следующей фразе диалога
                    if (currentDialog.currentPhraseNum >= currentDialog.phrases.Length)
                    {
                        //Если фраза была последняя, значит завершаем диалог
                        //Сохраняем номер диалога который у нас был
                        currentAction.currentDialogNum = (currentAction.currentDialogNum + 1) % currentAction.dialogs.Length;

                        //стираем текст из текстбокса и убираем фон
                        textBackground.SetActive(false);
                        bubbleText.text = "";
                        
                        state = PersonState.DialogFinished;
                    }
                    else
                    {
                        //Если не последняя, значит переходим к следующей
                        Phrase ph = currentDialog.phrases[currentDialog.currentPhraseNum];
                        ph.sayer.say(ph.speech);
                        currentDialog.currentPhraseNum += 1;
                    }
                }
                break;
                
            //Если диалог закончился, тогда переходим к следующему действию
            case PersonState.DialogFinished:
                if (currentAction.byTimer && Time.fixedTime - timeOfStart > currentAction.targetTimer)
                {
                    goToNextAction();
                }
                else if (!currentAction.byTimer && navigator.targetReached)
                {
                    goToNextAction();
                }
                break;
        }
        
        textBackground.gameObject.transform.rotation = Quaternion.identity; //new Quaternion(0,0,0,1);
        bubbleText.gameObject.transform.rotation = Quaternion.identity;

    }
    
    protected virtual void preFinishOfCurrentAction()
    {
        Debug.Log("Acting person pre finish of current Action");
    }

    protected virtual void goToNextAction()
    {
        preFinishOfCurrentAction();
        Debug.Log("Acting person next Action");
    }

    //Начиная новое действие, мы должны не только установить что нужно идти к новой точке, 
    //но и обозначить диалог который будем говорить когда дойдём до места
    //Сохранив при этом что диалог предыдущего действия продлился на 1
    public void setAction(PersonAct newAction)
    {
        navigator.SetTarget(newAction.target, newAction.targetDistance, newAction.talkingWithPerson);
                    
        if (newAction.dialogs.Length != 0)
        {
            currentDialog = newAction.dialogs[newAction.currentDialogNum];
            state = PersonState.ReadyForDialog;
        }
        else
            state = PersonState.DialogFinished;

        currentAction = newAction;
        if (currentAction.byTimer)
            timeOfStart = Time.fixedTime;
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
    ReadyForDialog,
    InDialog,
    DialogFinished
}