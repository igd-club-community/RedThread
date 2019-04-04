using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PersonAct
{
    public string name;
    public Transform target;
    public float targetDistance;
    public bool talkingWithPerson;
    public bool byTimer;
    public float targetTimer; //Время должно зависеть от количества диалогов
    public string animationName; //TODO: can be changed to think that convinient to describe animation
    public string[] phrases;
    public int currentDialogNum;
    public Phrases[] phrasesStorage;
    public Dialog[] dialogs;
}

[System.Serializable]
public class Phrases
{
    public string[] phrases;
}