using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevelNet : NetworkBehaviour
{
    public enum Mode {Save,Reset,Initialize};

    public Mode mode = Mode.Reset;

    // EnsureInit boolean
    private bool hasBeenSetup = false;

    public override void OnStartClient()
    {
        EnsureInit();
        base.OnStartClient();
    }

    // This is called everywhere a function might be called by the server before Awake()
    void EnsureInit(bool force = false)
    {
        if (!hasBeenSetup || force)
        {
            // Initialization code goes here

            hasBeenSetup = true;
        }
    }

    // Local logic
    void Awake()
    {
        EnsureInit();
    }

    public void ResetLevel()
    {
        if(hasBeenSetup)
        {
            CmdResetLevel();
        }
    }

    [Command(requiresAuthority = false)]
    void CmdResetLevel()
    {
        Resettable.ResetAll();
    }

    [Command(requiresAuthority = false)]
    void CmdResetLevelToInitial()
    {
        Resettable.ResetAllToInitial();
    }

    [Command(requiresAuthority = false)]
    void CmdSaveLevel()
    {
        Resettable.SaveAll();
    }

    void OnTriggerEnter()
    {
        if(hasBeenSetup)
        {
            switch(mode)
            {
                case Mode.Save:
                    CmdSaveLevel();
                    break;
                case Mode.Reset:
                    CmdResetLevel();
                    break;
                case Mode.Initialize:
                    CmdResetLevelToInitial();
                    break;
            }

        }
    }
}
