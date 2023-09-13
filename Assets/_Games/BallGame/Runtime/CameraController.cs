using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float m_smoothTime = 3f;
    [SerializeField] private float m_cameraOffset = 2.8f;

    private Vector3 targetPosition;
    private Vector3 currentVelocity;

    private void Awake()
    {
        targetPosition = transform.position;
    }

    public void SetCameraHeight (float y)
    {
        targetPosition = new Vector3(
            transform.position.x, 
            y + m_cameraOffset,
            transform.position.z);
    }

    private void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref currentVelocity,
            m_smoothTime);
            //Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * m_moveSpeed);
    }
}
