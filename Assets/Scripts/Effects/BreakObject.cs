using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        transform.parent.Find("Windows_broken").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
