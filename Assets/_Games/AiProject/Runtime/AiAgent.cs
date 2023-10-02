using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class AiAgentStats
{
    public float Energy;
    public float Food;
    public float Water;
}

public class AiAgent : MonoBehaviour
{
    [SerializeField] private NavMeshAgent m_navMeshAgent;
    [SerializeField] private Animator m_animator;
    [SerializeField] private ParticleSystem m_outOfEnergyExplosion;

    [Space]
    [SerializeField] private AiAgentStats m_stats;

    [Header("Audio")]
    [SerializeField] private AudioSource m_footStepSoud;

    [Header("AI")]
    [SerializeField] private ScriptableAiBehaviour[] m_aiBehaviours;

    public NavMeshAgent NavAgent => m_navMeshAgent;
    public float CurrentEnergy => m_stats.Energy;

    public Blackboard Blackboard { get; private set; } = new Blackboard();

    private AiCommandedByPlayer _aiMoveToTarget = new AiCommandedByPlayer();

    private IAiBehaviour _currentBehaviour;

    private List<IAiBehaviour> _potentialBehaviours = new List<IAiBehaviour>();

    private void Awake()
    {
        _potentialBehaviours.AddRange(m_aiBehaviours);
        SwitchBehaviour(m_aiBehaviours[0]);
    }

    private void Update()
    {
        m_stats.Energy -= Time.deltaTime;
        m_stats.Food -= Time.deltaTime;
        m_stats.Water -= Time.deltaTime;

        if (m_stats.Energy <= 0)
        {
            m_outOfEnergyExplosion.transform.parent = null;
            m_outOfEnergyExplosion.Play();
            gameObject.SetActive(false);
        }

        var currentBestBehaviour = FindBestAiBehviour();

        if (_currentBehaviour != currentBestBehaviour)
        {
            SwitchBehaviour(currentBestBehaviour);
        }

        if (_currentBehaviour.Execute(this))
        {
            Debug.Log("Need to replan");
            currentBestBehaviour = FindBestAiBehviour();
            SwitchBehaviour(currentBestBehaviour);
        }

        m_animator.SetFloat("Speed", m_navMeshAgent.velocity.magnitude);
    }

    private IAiBehaviour FindBestAiBehviour()
    {
        var currentBestScore = _currentBehaviour.Evaluate(this);
        var currentBestBehaviour = _currentBehaviour;

        foreach (var behaviour in _potentialBehaviours)
        {
            var score = behaviour.Evaluate(this);
            if (score > currentBestScore)
            {
                currentBestScore = score;
                currentBestBehaviour = behaviour;
            }
        }

        return currentBestBehaviour;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_currentBehaviour.GetType() != typeof(AiSeekResource))
        {
            return;
        }

        if (!Blackboard.TryGetGameObject("TargetOrb", out var targetOrb) 
            || targetOrb != other.gameObject)
        {
            return;
        }

        if (other.TryGetComponent<CollectableResource> (out var resource))
        {
            other.gameObject.SetActive(false);
            TryAddStat(
                resource.ResourceType.AssociatedStatName,
                resource.ResourceAmountToAdd);
        }
    }

    public void CommandToMoveTo(Vector3 target)
    {
        m_navMeshAgent.destination = target;
        SwitchBehaviour(_aiMoveToTarget);
    }

    public bool GetIsAtDestination()
    {
        return Vector3.Distance(m_navMeshAgent.destination, transform.position) < m_navMeshAgent.stoppingDistance + 0.1f;
    }

    public bool TryGetStat(string statName, out float value)
    {
        var field = typeof(AiAgentStats).GetField(statName);
        if (field == null)
        {
            Debug.Log($"AiAgentStats does not contain a field with name: {statName}");

            value = 0f;
            return false;
        }

        value = (float)field.GetValue(m_stats);
        return true;
    }

    public bool TryAddStat(string statName, float valueToAdd)
    {
        var field = typeof(AiAgentStats).GetField(statName);
        if (field == null)
        {
            Debug.Log($"AiAgentStats does not contain a field with name: {statName}");
            return false;
        }

        var current = (float)field.GetValue(m_stats);
        current += valueToAdd;
        field.SetValue(m_stats, current);
        return true;
    }

    private void OnValidate()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void SwitchBehaviour(IAiBehaviour toBehavior)
    {
        Debug.Log($"AI Switched state from: {_currentBehaviour} -> {toBehavior}");

        _currentBehaviour?.OnExit(this);
        _currentBehaviour = toBehavior;
        toBehavior.OnEnter(this);
    }

    private void OnFootStep()
    {
        m_footStepSoud.volume = Random.Range(0.6f, 0.75f);
        m_footStepSoud.pitch = Random.Range(0.9f, 1.1f);
        m_footStepSoud.Play();
    }
}
