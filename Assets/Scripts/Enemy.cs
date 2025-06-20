using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform[] _targets;

    [SerializeField] private float _pauseTime;

    private NavMeshAgent _agent;

    private Transform _player;

    [SerializeField] private float visionRange = 10.0f, visionAngle = 100.0f;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = FindFirstObjectByType<PlayerMovement>().transform;

        if (_targets != null && _targets.Length > 0) StartCoroutine(Patrolling());
        else Debug.LogError("Нет или недостаточно точек патруля");
    }

    private void Update()
    {
        TryFindPlayer();
    }

    private IEnumerator Patrolling()
    {
        while (true)
        {
            foreach (Transform target in _targets)
            {
                _agent.SetDestination(target.position);

                while (_agent.pathPending || _agent.remainingDistance > _agent.stoppingDistance) // ждём, пока игрок дойдёт до нужной точки и страхуемся, если путь ещё не построен
                {
                    yield return null;
                }

                yield return new WaitForSeconds(_pauseTime);
            }
        }
    }

    private void TryFindPlayer()
    {
        if (Vector3.Distance(transform.position, _player.position) <= visionRange)
        {
            Vector3 directionToTarget = _player.position - transform.position;
            directionToTarget.y = 0;

            float angleToTarget = Vector3.SignedAngle(directionToTarget, transform.forward, Vector3.up);

            if (MathF.Abs(angleToTarget) <= visionAngle)
            {
                GetComponent<Collider>().enabled = false;
                if (Physics.Raycast(transform.position, _player.position - transform.position, out RaycastHit hit, visionRange) && hit.transform == _player)
                {
                    print(1);
                    _agent.ResetPath();
                    StopAllCoroutines();
                }
                GetComponent<Collider>().enabled = true;
            }
        }
    }
}