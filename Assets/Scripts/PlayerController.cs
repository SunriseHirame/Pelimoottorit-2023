using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_launchForce = 10f;

    private Rigidbody2D rigidbody;

    private float timeBoosted;
    private bool launch;
    private Vector2 launchDirection;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
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
            var direction = (transform.position - screenToWorldPoint).normalized;
            launchDirection = direction;
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
            SceneManager.LoadScene(gameObject.scene.buildIndex);
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
