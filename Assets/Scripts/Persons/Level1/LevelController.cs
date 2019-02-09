using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

    public delegate void PersonEvent();
    //Какие события босс может сгенерировать?
    public event PersonEvent BossNeedsCoffee;
    public event PersonEvent CoffeeDelivered;
    public event PersonEvent SecretaryIsBack;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void generateNeedCoffeEvent()
    {
        BossNeedsCoffee();
    }
    public void generateCoffeeDelivered()
    {
        CoffeeDelivered();
    }
    public void generateSecretaryIsBack()
    {
        SecretaryIsBack();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
