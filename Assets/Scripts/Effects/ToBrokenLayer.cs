using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToBrokenLayer : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer != 10)
        {
            GetComponent<Rigidbody>().useGravity = true    ;
            Invoke("Broken", 0.2f);
        }
    }

    private void Broken()
    {
        gameObject.layer = 11;
    }
}
