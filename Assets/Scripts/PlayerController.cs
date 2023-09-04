using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_launchForce = 10f;


    [SerializeField] private int m_startingHealth = 3;

    private Rigidbody2D rigidbody;

    private float timeBoosted;
    private bool launch;
    private Vector2 launchDirection;

    private int currentHealth;

    public int CurrentHealth => currentHealth;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        currentHealth = m_startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            timeBoosted += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0))
        {
            launch = true;
            var screenToWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var direction = (transform.position - screenToWorldPoint);
            direction.z = 0f;
            launchDirection = direction.normalized;
        }
    }

    public void GetHurt(int amount = 1)
    {
        if (amount <= 0) return;

        Debug.Log("Player got hurt");
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            StartCoroutine(ReloadLevel());
        }
    }

    private void FixedUpdate()
    {
        if (launch)
        {
            Debug.Log("Launch Ball");
            var launchForce = launchDirection * timeBoosted * m_launchForce;
            rigidbody.AddForce(launchForce, ForceMode2D.Impulse);
            timeBoosted = 0;
            launch = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Goal"))
        {
            if (collision.gameObject.TryGetComponent<Goal> (out var goal) && !string.IsNullOrEmpty (goal.NextLevelName))
            {
                SceneManager.LoadScene (goal.NextLevelName);
            }
            else
            {
                var sceneIndexToLoad = gameObject.scene.buildIndex;
                sceneIndexToLoad %= SceneManager.sceneCountInBuildSettings;
                SceneManager.LoadScene(sceneIndexToLoad);
            }
        }
    }

    [ContextMenu("Just Die")]
    private void WouldYouKindlyJustDie()
    {
        GetHurt(currentHealth);
    }

    private IEnumerator ReloadLevel()
    {
        var handle = SceneManager.LoadSceneAsync(gameObject.scene.buildIndex);
        handle.allowSceneActivation = false;
        Time.timeScale = 0.1f;

        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 1f;
        handle.allowSceneActivation = true;
    }
}
