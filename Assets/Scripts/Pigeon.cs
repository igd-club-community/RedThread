using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pigeon : MonoBehaviour
{
    public Vector3 force = new Vector3(0f, 0f, 0f);
    public bool doNow = true;
    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (doNow)
        {
            _rigidbody.AddForce(force, ForceMode.Impulse);
            doNow = false;
        }
    }
}