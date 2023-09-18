using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.CompareTag("Player"))
        {
            Debug.Log("Player exited platform");
            if (transform.position.y < collision.transform.position.y)
            {
                Debug.Log("Player went over a platform");
                Camera.main.GetComponent<CameraController>().SetCameraHeight(transform.position.y);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("COllided");
    }
}
