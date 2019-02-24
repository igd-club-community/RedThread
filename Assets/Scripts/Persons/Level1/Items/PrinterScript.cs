using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinterScript : ItemScript
{
    public Level1States states;

    public override void Act()
    {
        Debug.Log("PrinterScript");

        states.PaperInPrinter = !states.PaperInPrinter;
    }
}
