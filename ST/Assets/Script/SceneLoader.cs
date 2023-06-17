using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>

    
{
    public string sceneNameToBeLoaded;
    public void LoadScene(string _sceneName)
    {
        sceneNameToBeLoaded = _sceneName;

        StartCoroutine(InitializeSceneLoading());

    }

    IEnumerator InitializeSceneLoading()
    {
        yield return SceneManager.LoadSceneAsync("Scene_Loading");

        StartCoroutine(LoadAcutualScene());
    }

    IEnumerator LoadAcutualScene()
    {
       var asynSceneLoading = SceneManager.LoadSceneAsync(sceneNameToBeLoaded);
        asynSceneLoading.allowSceneActivation = false;
        while (!asynSceneLoading.isDone)
        {
            if (asynSceneLoading.progress >= 0.9f)
            {
                asynSceneLoading.allowSceneActivation = true ;
            }
            yield return null;
        }
        
    }
}


