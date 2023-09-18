using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;
 

public class HelloWorldScoreView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_playerScoreEntryPrefab;

    private List<TextMeshProUGUI> _playerScoreEntries = new List<TextMeshProUGUI>();

    private void LateUpdate()
    {
        // Store the used count of score entry texts
        var i = 0;

        foreach (var playerGo in GameObject.FindGameObjectsWithTag("Player"))
        {
            var player = playerGo.GetComponent<HelloWorldPlayer>();
            var isLocalPlayer = player.IsLocalPlayer; // player.gameObject == NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();


            if (_playerScoreEntries.Count <= i)
            {
                _playerScoreEntries.Add(Instantiate(m_playerScoreEntryPrefab, transform));
            }

            _playerScoreEntries[i].text = $"{player.gameObject.name}: {player.Score}";
            _playerScoreEntries[i].gameObject.SetActive(true);
            _playerScoreEntries[i].color = isLocalPlayer ? Color.green : Color.white;

            i++;
        }

        // Disable extra player score entry texts
        for (; i < _playerScoreEntries.Count; i++)
        {
            _playerScoreEntries[i].gameObject.SetActive(false);
        }
    }
}
