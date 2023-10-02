using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public interface IGrindable
{
    Vector3 GetPointOnSpline(Vector3 fromPosition, float distanceForward);
}

public class SplineFollower : MonoBehaviour
{
    private IGrindable _splineToFollow;

    private void Update()
    {
        if (_splineToFollow != null)
        {
            // Follow the spline
        }
        else
        {
            // Move in some other ways
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IGrindable>(out var spline))
        {
            // Enter something we can grind on
            _splineToFollow = spline;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IGrindable>(out var spline))
        {
            // Exited from something we can grind on

            if (_splineToFollow == spline) _splineToFollow = default;
        }
    }
}
