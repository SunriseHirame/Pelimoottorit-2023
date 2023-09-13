using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode _)
    {
        if (scene.name.StartsWith("Level_"))
        {
            Debug.Log($"Loaded a level! {scene.name}");
            PlayerPrefs.SetString("LastLevel", scene.name);
            PlayerPrefs.Save();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
