using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    void OnMouseOver()
    {
        Debug.Log(gameObject.name);
    }
}
