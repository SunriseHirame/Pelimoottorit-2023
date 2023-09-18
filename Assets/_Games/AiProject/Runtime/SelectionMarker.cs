using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionMarker : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Selected()
    {
        gameObject.SetActive(true);
    }

    public void Deselected()
    {
        gameObject.SetActive(false);
    }
}
