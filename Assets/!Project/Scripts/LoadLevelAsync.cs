using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class LoadLevelAsync : MonoBehaviour
{
    public string sceneName;

    void Start()
    {
        #if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
        #endif

        Application.backgroundLoadingPriority = ThreadPriority.Low;
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        AsyncOperation AO = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Single); 
        AO.allowSceneActivation = false;
        while(AO.progress < 0.9f)
        {
            yield return null;
        }

        //Fade the loading screen out here

        AO.allowSceneActivation = true;
    }
}
