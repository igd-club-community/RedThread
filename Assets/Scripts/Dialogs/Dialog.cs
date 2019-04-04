using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    public int currentPhraseNum;
    public Phrase[] phrases;
}

[System.Serializable]
public class Phrase
{
    public ActingPerson sayer;
    public string speech;
}