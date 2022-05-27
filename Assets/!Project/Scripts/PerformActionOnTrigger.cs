using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PerformActionOnTrigger : MonoBehaviour
{
    public UnityEvent action;

    void OnTriggerEnter()
    {
        action.Invoke();
    }
}
