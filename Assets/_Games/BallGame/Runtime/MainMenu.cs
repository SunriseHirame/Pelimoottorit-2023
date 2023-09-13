using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string m_firstLevelName = "Level_001";

    [Header ("Views")]
    [SerializeField] private GameObject m_mainView;
    [SerializeField] private GameObject m_levelSelectView;

    public void Continue()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("LastLevel", m_firstLevelName));
    }

    public void LoadLevel (string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void OpenMainView()
    {
        m_mainView.SetActive(true);
        m_levelSelectView.SetActive(false);
    }

    public void OpenSelectLevelView()
    {
        m_mainView.SetActive(false);
        m_levelSelectView.SetActive(true);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
