using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAiBehaviour
{
    float Evaluate(AiAgent agent);
    bool Execute(AiAgent agent);
}

public class AiWanderAround : IAiBehaviour
{
    private float _timeToWait;

    public float Evaluate(AiAgent agent)
    {
        return agent.CurrentEnergy;
    }

    public bool Execute(AiAgent agent)
    {
        if (_timeToWait > 0)
        {
            _timeToWait -= Time.deltaTime;
            return false;
        }

        if (!agent.GetIsAtDestination())
        {
            return false;
        }

        var targetPosition = new Vector3(Random.Range(-48, 48), 0f, Random.Range(-48, 48));
        agent.NavAgent.destination = targetPosition;

        _timeToWait = Random.Range(3f, 5f);
        return false;
    }
}

public class AiMoveToTarget : IAiBehaviour
{
    public float Evaluate(AiAgent agent)
    {
        return float.MaxValue;
    }

    public bool Execute (AiAgent agent)
    {
        if (!agent.GetIsAtDestination())
        {
            return false;
        }

        return true;
    }
}

public class AiSeekEnergy : IAiBehaviour
{
    private GameObject _targetedOrb;

    public float Evaluate(AiAgent agent)
    {
        if (agent.CurrentEnergy > 60) return 0;
        return (60 - agent.CurrentEnergy) * 10f;
    }

    public bool Execute(AiAgent agent)
    {
        if (!_targetedOrb || !_targetedOrb.activeInHierarchy)
        {
            var allEnergyOrbs = GameObject.FindGameObjectsWithTag("EnergyOrb");

            if (allEnergyOrbs.Length == 0)
            {
                Debug.Log("NO ORBS!");
                return true;
            }

            var closestDistanceToOrb = float.MaxValue;
            var closestOrb = default(GameObject);

            for (int i = 0; i < allEnergyOrbs.Length; i++)
            {
                var distance = Vector3.Distance(agent.transform.position, allEnergyOrbs[i].transform.position);
                if (distance < closestDistanceToOrb)
                {
                    closestOrb = allEnergyOrbs[i];
                    closestDistanceToOrb = distance;
                }
            }

            Debug.Log("Found target orb");
            _targetedOrb = closestOrb;
            agent.NavAgent.destination = _targetedOrb.transform.position;
        }


        return agent.GetIsAtDestination();
    }
}