using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour
{
    private Vector2 m_offsetEndPosition = new Vector2(5, 0);
    private Rigidbody2D rigidbody;
    private Vector2 startPosition;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        startPosition = rigidbody.position;
    }

    private void FixedUpdate()
    {
        var t = Mathf.PingPong(Time.timeSinceLevelLoad, 1f);
        var position = Vector2.Lerp(startPosition, startPosition + m_offsetEndPosition, t);
        rigidbody.MovePosition(position);
    }
}
