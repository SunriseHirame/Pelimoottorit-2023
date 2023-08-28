using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_launchForce = 10f;

    private Rigidbody2D _rigidbody;

    private float _timeBoosted;
    private bool _launch;
    private Vector2 _launchDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _timeBoosted += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _launch = true;
            var screenToWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var direction = transform.position - screenToWorldPoint;
            direction.z = 0;
            _launchDirection = direction.normalized;
        }
    }

    private void FixedUpdate()
    {
        if (_launch)
        {
            Debug.Log("Launch Ball");
            var launchForce = _launchDirection * (_timeBoosted * m_launchForce);
            _rigidbody.AddForce(launchForce, ForceMode2D.Impulse);
            _timeBoosted = 0;
            _launch = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Goal> (out var goal))
        {
            if (string.IsNullOrEmpty (goal.NextLevelName))
            {
                var sceneIndexToLoad = gameObject.scene.buildIndex + 1;
                sceneIndexToLoad %= SceneManager.sceneCountInBuildSettings;
                SceneManager.LoadScene(sceneIndexToLoad);
            }
            else
            {
                SceneManager.LoadScene (goal.NextLevelName);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.attachedRigidbody && collision.collider.attachedRigidbody.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(gameObject.scene.buildIndex);
        }
    }
}
