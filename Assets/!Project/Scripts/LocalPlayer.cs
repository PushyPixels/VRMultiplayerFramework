using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : MonoBehaviour
{
    public NetworkPlayer networkPlayer;

    public static LocalPlayer instance;

    void Awake()
    {
        instance = this;
    }
}
