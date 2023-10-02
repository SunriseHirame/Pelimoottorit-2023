using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public static class Logger
{
    [Conditional("DEVELOPMENT_BUILD")]
    public static void Log(string message)
    {
        UnityEngine.Debug.LogError(message);
    }
}

public class GameManager : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.BeforeSceneLoad)] 
    private static void Init()
    {
        var gameManager = new GameObject("Game Manager", typeof (GameManager));
        DontDestroyOnLoad(gameManager);
    }

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Logger.Log("MESSAGE");
#if !UNITY_EDITOR
        Debug.unityLogger.filterLogType = LogType.Error | LogType.Assert;
#endif
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode _)
    {
        if (scene.name.StartsWith("Level_"))
        {
            UnityEngine.Debug.Log($"Loaded a level! {scene.name}");
            PlayerPrefs.SetString("LastLevel", scene.name);
            PlayerPrefs.Save();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
