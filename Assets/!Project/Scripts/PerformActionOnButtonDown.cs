using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PerformActionOnButtonDown : MonoBehaviour
{
    public string buttonName = "Fire1";
    public UnityEvent action;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown(buttonName))
        {
            action.Invoke();
        }
    }
}
