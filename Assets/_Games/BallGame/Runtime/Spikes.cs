using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float m_hurtArc = 30f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.attachedRigidbody) return;
        if (!collision.attachedRigidbody.TryGetComponent<PlayerController>(out var player)) return;

        var collisionAngle = Vector2.Angle(-transform.up, collision.attachedRigidbody.velocity);
        if (collisionAngle * 2f < m_hurtArc)
        {
            player.GetHurt();
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(0, 0, m_hurtArc / 2F) * Vector3.up);
        Gizmos.DrawLine(Vector3.zero, Quaternion.Euler(0, 0, -m_hurtArc / 2F) * Vector3.up);
    }
}
