﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PersonAct
{
    public string name;
    public Transform target;
    public float targetDistance;
    public bool byTimer;
    public float targetTimer; //Время должно зависеть от количества диалогов
    public string animationName; //TODO: can be changed to think that convinient to describe animation
    public string[] phrases;
    public int phraseStageNum;
    public Phrases[] phrasesStorage;
}

[System.Serializable]
public class Phrases
{
    public string[] phrases;
}