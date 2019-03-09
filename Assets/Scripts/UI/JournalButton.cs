using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalButton : MonoBehaviour
{

    public GameObject journalPaper;

    private void Start()
    {
        journalPaper.SetActive(false);
    }

    public void ShowJournal()
    {
        journalPaper.SetActive(true);
    }
}
