using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WanderAroundMode
{
    WorldSpaceArea,
    PointNearAgent,
}

[CreateAssetMenu(menuName = "Ai Project/Ai Wander Around")]
public class AiWanderAround : ScriptableAiBehaviour
{
    [SerializeField] private WanderAroundMode m_wanderMode = WanderAroundMode.WorldSpaceArea;
    [SerializeField] private Vector3 m_wanderAreaSize = new Vector3(48, 0, 48);

    [SerializeField] private float m_minTimeToWaitAtTarget = 3f;
    [SerializeField] private float m_maxTimeToWaitAtTarget = 5f;

    public override float Evaluate(AiAgent agent)
    {
        return agent.CurrentEnergy;
    }

    public override void OnEnter(AiAgent agent)
    {
        var targetPosition = new Vector3(
            Random.Range(-m_wanderAreaSize.x, m_wanderAreaSize.x), 
            0f, 
            Random.Range(-m_wanderAreaSize.z, m_wanderAreaSize.z));

        agent.NavAgent.destination = m_wanderMode switch
        {
            WanderAroundMode.WorldSpaceArea => targetPosition,
            WanderAroundMode.PointNearAgent => agent.transform.position + targetPosition,
            _ => targetPosition,
        };

        /* This does the same as the switch expression above
        switch (m_wanderMode)
        {
            case WanderAroundMode.WorldSpaceArea:
                agent.NavAgent.destination = targetPosition;
                break;
            case WanderAroundMode.PointNearAgent:
                agent.NavAgent.destination = agent.transform.position + targetPosition;
                break;
            default:
                break;
        }
        */

        agent.Blackboard.WriteValue("TimeToWait", Random.Range(m_minTimeToWaitAtTarget, m_maxTimeToWaitAtTarget));
        //_timeToWait = Random.Range(3f, 5f);
    }

    public override bool Execute(AiAgent agent)
    {
        if (!agent.GetIsAtDestination())
        {
            return false;
        }

        if ((float) agent.Blackboard["TimeToWait"] > 0)
        {
            if (agent.Blackboard.TryGetFloat("TimeToWait", out var timeLeftToWait))
            {
                timeLeftToWait -= Time.deltaTime;
                agent.Blackboard.WriteValue("TimeToWait", timeLeftToWait);
                return false;
            }
        } 

        return true;
    }
}