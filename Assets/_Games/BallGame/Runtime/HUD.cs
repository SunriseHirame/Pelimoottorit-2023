using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class HUD : MonoBehaviour
{
    [SerializeField] private Transform m_heartsParent;
    [SerializeField] private Color m_fullHeart;
    [SerializeField] private Color m_lostHeart;

    [Space]
    [SerializeField] private GameObject m_endScreen;

    private List<Image> hearts = new List<Image>();
    private PlayerController player;

    private void Awake()
    {
        m_heartsParent.GetComponentsInChildren<Image>(hearts);
        hearts.RemoveAt(0);

        player = FindAnyObjectByType<PlayerController>();
    }


    private void LateUpdate()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].color = i < player.CurrentHealth ? m_fullHeart : m_lostHeart;
        }

        if (player.CurrentHealth <= 0) m_endScreen.SetActive(true);
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying) return;
#endif

        m_heartsParent.GetComponentsInChildren<Image>(hearts);
        hearts.RemoveAt(0);
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].color = i < hearts.Count - 1 ? m_fullHeart : m_lostHeart;
        }
    }
}
