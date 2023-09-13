using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AiAgent : MonoBehaviour
{
    [SerializeField] private NavMeshAgent m_navMeshAgent;

    [SerializeField] private float m_startingEnergy = 100f;
    [SerializeField] private ParticleSystem m_outOfEnergyExplosion;

    public NavMeshAgent NavAgent => m_navMeshAgent;
    public float CurrentEnergy => _currentEnergy;

    private AiWanderAround _aiWanderAround = new AiWanderAround();
    private AiSeekEnergy _aiSeekEnergy = new AiSeekEnergy();
    private AiMoveToTarget _aiMoveToTarget = new AiMoveToTarget();

    private IAiBehaviour _currentBehaviour;

    private List<IAiBehaviour> _potentialBehaviours = new List<IAiBehaviour>();


    private float _currentEnergy;

    private void Awake()
    {
        _potentialBehaviours.Add(_aiWanderAround);
        _potentialBehaviours.Add(_aiSeekEnergy);

        _currentEnergy = m_startingEnergy;
        SwitchBehaviour(_aiWanderAround);
    }

    private void Update()
    {
        _currentEnergy -= Time.deltaTime;

        if (_currentEnergy <= 0)
        {
            m_outOfEnergyExplosion.transform.parent = null;
            m_outOfEnergyExplosion.Play();
            gameObject.SetActive(false);
        }

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

        if (_currentBehaviour != currentBestBehaviour)
        {
            SwitchBehaviour(currentBestBehaviour);
        }

        if (_currentBehaviour.Execute(this))
        {
            Debug.Log("Need to replan");
            SwitchBehaviour(_aiWanderAround);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnergyOrb"))
        {
            other.gameObject.SetActive(false);
            _currentEnergy = m_startingEnergy;
        }
    }

    public void SetDestination(Vector3 target)
    {
        m_navMeshAgent.destination = target;
        SwitchBehaviour(_aiMoveToTarget);
    }

    public bool GetIsAtDestination()
    {
        return Vector3.Distance(m_navMeshAgent.destination, transform.position) < m_navMeshAgent.stoppingDistance + 0.1f;
    }


    private void OnValidate()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void SwitchBehaviour(IAiBehaviour toBehavior)
    {
        Debug.Log($"AI Switched state from: {_currentBehaviour} -> {toBehavior}");
        _currentBehaviour = toBehavior;

    }
}
