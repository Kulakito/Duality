using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private Transform[] _targets;
    private int _currentTarget = 0;
    
    [SerializeField] private float _pauseTime, _alertTime;
    [SerializeField] private float visionRange = 10.0f, visionAngle = 100.0f;

    private NavMeshAgent _agent;
    private Transform _player;

    private Coroutine _currentRoutine;

    private enum State { Patrolling, LookingAtPlayer }
    private State _currentState;

    [SerializeField] private Transform _alertCanvas;
    [SerializeField] private Image _alertImage;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = FindFirstObjectByType<PlayerMovement>().transform;

        if (_targets != null && _targets.Length > 0) SwitchState(State.Patrolling);
        else Debug.LogError("Нет или недостаточно точек патруля");
    }

    private void Update()
    {
        _alertCanvas.LookAt(Camera.main.transform);

        if (_currentState == State.Patrolling && TryFindPlayer())
        {
            SwitchState(State.LookingAtPlayer);
        }
        else if (_currentState == State.LookingAtPlayer && !TryFindPlayer())
        {
            SwitchState(State.Patrolling);
        }
    }

    private void SwitchState(State newState)
    {
        if (_currentRoutine != null)
        {
            _agent.ResetPath();
            StopCoroutine(_currentRoutine);
        }

        _currentState = newState;

        switch (newState)
        {
            case State.Patrolling:
                _currentRoutine = StartCoroutine(Patrolling());
                break;
            case State.LookingAtPlayer:
                _currentRoutine = StartCoroutine(LookAtPlayer());
                break;
        }
    }

    private IEnumerator Patrolling()
    {
        _alertImage.fillAmount = 0;

        while (true)
        {
            for (int i = _currentTarget;  i < _targets.Length; i++)
            {
                _agent.SetDestination(_targets[i].position);

                yield return new WaitUntil(() => !(_agent.pathPending || _agent.remainingDistance > _agent.stoppingDistance)); // ждём, пока игрок дойдёт до нужной точки и страхуемся, если путь ещё не построен

                yield return new WaitForSeconds(_pauseTime);

                _currentTarget++;
            }
            _currentTarget = 0;
        }
    }

    private IEnumerator LookAtPlayer()
    {
        while (_currentState == State.LookingAtPlayer)
        {
            Vector3 direction = (_player.position - transform.position).normalized;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _alertTime);
                _alertImage.fillAmount = Mathf.Lerp(_alertImage.fillAmount, 1, Time.deltaTime * _alertTime);
            }

            yield return null;
        }
    }

    private bool TryFindPlayer()
    {
        if (Vector3.Distance(transform.position, _player.position) <= visionRange)
        {
            Vector3 directionToTarget = _player.position - transform.position;
            directionToTarget.y = 0;

            float angleToTarget = Vector3.SignedAngle(directionToTarget, transform.forward, Vector3.up);

            if (MathF.Abs(angleToTarget) <= visionAngle)
            {
                GetComponent<Collider>().enabled = false;
                if (Physics.Raycast(transform.position, _player.position - transform.position, out RaycastHit hit, visionRange) && hit.collider.GetComponentInParent<PlayerMovement>() && !hit.collider.GetComponentInParent<PlayerVisibility>().isGhost)
                {
                    GetComponent<Collider>().enabled = true;
                    return true;
                }
                GetComponent<Collider>().enabled = true;
            }
        }
        return false;
    }
}