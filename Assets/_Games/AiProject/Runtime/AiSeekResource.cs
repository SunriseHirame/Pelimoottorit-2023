using UnityEngine;

[CreateAssetMenu(menuName = "Ai Project/Ai Seek Energy")]
public class AiSeekResource : ScriptableAiBehaviour
{
    [SerializeField] private ResoureType m_resourceToSearchFor;
    [SerializeField] private AnimationCurve m_behaviourWeightCurve = AnimationCurve.Linear(0f, 100f, 100f, 0f);

    public override float Evaluate(AiAgent agent)
    {
        if (!CollectableResource.TryGetResourcesOfType (m_resourceToSearchFor, out var _)) return float.MinValue;

        if (!agent.TryGetStat(m_resourceToSearchFor.AssociatedStatName, out var statValue)) return float.MinValue;

        //if (agent.CurrentEnergy > 60) return 0;
        //return (60 - agent.CurrentEnergy) * 10f;

        return m_behaviourWeightCurve.Evaluate(statValue);
    }

    public override void OnEnter(AiAgent agent)
    {
        if (!CollectableResource.TryGetResourcesOfType(m_resourceToSearchFor, out var allReourcesOfType))
        {
            Debug.Log("NO ORBS!");
            return;
        }

        var closestDistanceToOrb = float.MaxValue;
        var closestOrb = default(GameObject);

        for (int i = 0; i < allReourcesOfType.Count; i++)
        {
            var distance = Vector3.Distance(agent.transform.position, allReourcesOfType[i].transform.position);
            if (distance < closestDistanceToOrb)
            {
                closestOrb = allReourcesOfType[i].gameObject;
                closestDistanceToOrb = distance;
            }
        }

        Debug.Log("Found target orb");
        agent.Blackboard.WriteValue("TargetOrb", closestOrb);
        //_targetedOrb = closestOrb;
        agent.NavAgent.destination = closestOrb.transform.position;
    }

    public override bool Execute(AiAgent agent)
    {
        return agent.GetIsAtDestination();
    }
}