using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonDialogController : MonoBehaviour
{
    public bool ended = true;
    public float textMaxTime = 3; //время в течении которого отображается фраза
    public float sayTime = -3; //время начала отображения фразы

    public GameObject textBackground; //фон текста
    public Text bubbleText; //текстбокс
    public Dialog currentDialog;
    Phrase ph;

    // Start is called before the first frame update
    void Start()
    {
        ended = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (ended)
            return;

        //Если с момента сказания фразы прошло больше textMaxTime секунд
        if (Time.fixedTime - sayTime > textMaxTime)
        {
            //стираем текст из текстбокса предыдущей фразы и убираем фон
            if (ph != null)
                ph.sayer.cleanSay();

            //Тогда мы переходим к следующей фразе диалога
            if (currentDialog.currentPhraseNum >= currentDialog.phrases.Length)
            {
                ended = true;
                currentDialog.currentPhraseNum = 0;
                ph.sayer.cleanSay();
            }
            else
            {
                //Если не последняя, значит переходим к следующей
                ph = currentDialog.phrases[currentDialog.currentPhraseNum];
                ph.sayer.say(ph.speech);
                currentDialog.currentPhraseNum += 1;
                sayTime = Time.fixedTime;
            }
        }
    }

    public void StartDialog(Dialog currentDialog)
    {
        this.currentDialog = currentDialog;
        ended = false;
    }

    public void EndDialog()
    {
        if (ph != null)
            ph.sayer.cleanSay();
        ended = true;
    }
}
