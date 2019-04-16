using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject leftBar;
    public GameObject journalUI;
    public GameObject winScreen;
    public GameObject looseScreen;
    public GameObject shelvesIcon;
    public GameObject HeartIcon1;
    public GameObject HeartIcon2;
    public bool state = false;
    public void ChangeStateOfJournal()
    {
        Debug.Log("ChangeState");
        if (state)
        {
            disable();
        }
        else
        {
            enable();
        }
        state = !state;
        //target.SetActive(state);
    }
    public void enable()
    {
        journalUI.SetActive(true);
    }
    public void disable()
    {
        journalUI.SetActive(false);
    }
    

    public void enableHearts()
    {
        HeartIcon1.SetActive(true);
        HeartIcon2.SetActive(true);
    }
    public void enableWin()
    {
        leftBar.SetActive(false);
        journalUI.SetActive(false);
        looseScreen.SetActive(false);
        winScreen.SetActive(true);
    }
    public void enableLoose()
    {
        leftBar.SetActive(false);
        journalUI.SetActive(false);
        looseScreen.SetActive(true);
        winScreen.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
