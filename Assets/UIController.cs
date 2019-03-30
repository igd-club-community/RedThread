using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject target;
    public bool state = false;
    public void ChangeState()
    {
        Debug.Log("ChangeState");
        if (state)
        {
            enable();
        }
        else
        {
            disable();
        }
        state = !state;
        //target.SetActive(state);
    }
    public void enable()
    {
        target.SetActive(true);
    }
    public void disable()
    {
        target.SetActive(false);
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
