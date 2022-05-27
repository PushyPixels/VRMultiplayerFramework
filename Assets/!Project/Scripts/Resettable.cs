using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Resettable : MonoBehaviour
{
    private static List<Resettable> resetables = new List<Resettable>();

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Vector3 savedPosition;
    private Quaternion savedRotation;

    // Start is called before the first frame update
    void Awake()
    {
        resetables.Add(this);
    }

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void SaveNow()
    {
        savedPosition = transform.position;
        savedRotation = transform.rotation;
    }

    public static void SaveAll()
    {
        foreach(Resettable resettable in resetables)
        {
            resettable.SaveNow();
        }
    }

    public void ResetInitialNow()
    {
        if(GetComponent<SettableAuthority>())
        {
            GetComponent<SettableAuthority>().RemoveAuthority();
        }
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        if(GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    public static void ResetAllToInitial()
    {
        foreach(Resettable resettable in resetables)
        {
            resettable.ResetInitialNow();
        }
    }

    public void ResetNow()
    {
        if(GetComponent<SettableAuthority>())
        {
            GetComponent<SettableAuthority>().RemoveAuthority();
        }
        transform.position = savedPosition;
        transform.rotation = savedRotation;
        if(GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }

    public static void ResetAll()
    {
        foreach(Resettable resettable in resetables)
        {
            resettable.ResetNow();
        }
    }
}
