using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HapticsManager : MonoBehaviour
{
    public static HapticsManager instance;

    [System.Serializable]
    public class HapticPulse
    {
        public bool isRight = false;
        public uint channel = 0;
        public float amplitude = 1.0f;
        public float duration = 0.1f;
    }

    private List<InputDevice> leftHandDevices = new List<InputDevice>();
    private List<InputDevice> rightHandDevices = new List<InputDevice>();

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private List<InputDevice> GetHapticCapableDevices(InputDeviceCharacteristics hand)
    {
        List<InputDevice> allDevices = new List<InputDevice>();
        List<InputDevice> capableDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(hand, allDevices);

        foreach(InputDevice device in allDevices)
        {
            HapticCapabilities capabilities;
            if(device.TryGetHapticCapabilities(out capabilities))
            {
                if(capabilities.supportsImpulse)
                {
                    capableDevices.Add(device);
                }
            }
        }

        return capableDevices;
    }

    void Start()
    {
        leftHandDevices = GetHapticCapableDevices(InputDeviceCharacteristics.Left);
        rightHandDevices = GetHapticCapableDevices(InputDeviceCharacteristics.Right);
    }

    public void ApplyHapticImpulse(bool isRight, float amplitude, float duration)
    {
        List<InputDevice> devices;
        if(isRight)
        {
            devices = rightHandDevices;
        }
        else
        {
            devices = leftHandDevices;
        }

        foreach(InputDevice device in devices)
        {
            device.SendHapticImpulse(0, amplitude, duration); // No idea what "channel" means in this context, but 0 works!
        }
    }

    public void ApplyHapticImpulse(HapticPulse pulse)
    {
        List<InputDevice> devices;
        if(pulse.isRight)
        {
            devices = rightHandDevices;
        }
        else
        {
            devices = leftHandDevices;
        }

        foreach(InputDevice device in devices)
        {
            device.SendHapticImpulse(pulse.channel, pulse.amplitude, pulse.duration); // No idea what "channel" means in this context, but 0 works!
        }
    }
}
