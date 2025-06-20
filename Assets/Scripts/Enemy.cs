using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform[] _targets;

    [SerializeField] private float _pauseTime;

    private NavMeshAgent _agent;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        if (_targets != null && _targets.Length > 0) StartCoroutine(Patrolling());
        else Debug.LogError("Нет или недостаточно точек патруля");
    }

    private IEnumerator Patrolling()
    {
        while (true)
        {
            foreach (Transform target in _targets)
            {
                _agent.SetDestination(target.position);

                while (_agent.pathPending || _agent.remainingDistance > _agent.stoppingDistance)
                {
                    yield return null;
                }

                yield return new WaitForSeconds(_pauseTime);
            }
        }
    }
}