using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR.Haptics;
using Utils;
[RequireComponent(typeof(PolygonCollider2D))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State _startingState;
    [SerializeField] private float _roamingDistanceMax = 7f;
    [SerializeField] private float _roamingDistanceMin = 3f;
    [SerializeField] private float _roamingTimerMax = 2f;

    [SerializeField] private bool _isChasingEnemy = false;
    [SerializeField] private float _chasingDistance = 5f;
    [SerializeField] private float _chasingSpeedMultiplier = 1.5f;
    [SerializeField] private float _attackRate = 2f;
    private float _nextAttackTime = 0f;

    [SerializeField] private bool _isAttackngEnemy = false;
    [SerializeField] private float _attackingDistance = 2f;

    private NavMeshAgent _navMeshAgent;
    private State _CurrentState;
    private float _roamingTimer;
    private Vector3 _roamPosition;
    private Vector3 _startingPosition;

    private float _roamingSpeed;
    private float _chasingSpeed;

    public event EventHandler OnEnemyAttack;
    private enum State
    {
        Idle,
        Roaming,
        Attacking,
        Chasing,
        Death
    }
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _CurrentState = _startingState;
        _roamingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _navMeshAgent.speed * _chasingSpeedMultiplier;
    }

    private void Update()
    {
        StateHandler();
    }

    private void StateHandler()
    {
        switch (_CurrentState)
        {
            default:
            case State.Idle:

                break;
            case State.Roaming:
                _roamingTimer -= Time.deltaTime;
                if (_roamingTimer < 0)
                {
                    Roaming();
                    _roamingTimer = _roamingTimerMax;
                }
                CheckCurrentState();
                break;
            case State.Chasing:
                ChasingTarget();
                CheckCurrentState();
                break;
            case State.Attacking:
                AttackingTarget();
                CheckCurrentState();
                break;
            case State.Death:
                break;
        }
    }

    private void ChasingTarget()
    {
        _navMeshAgent.SetDestination(Player.Instance.transform.position);
    }

    public float GetRoamingAnimationSpeed()
    {
        return _navMeshAgent.speed / _roamingSpeed;
    }
    private void CheckCurrentState()
    {
        float distanceToPLayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        State newState = State.Roaming;
        if (_isChasingEnemy)
        {
            if (distanceToPLayer <= _chasingDistance)
            {
                newState = State.Chasing;
            }
        }
        if (_isAttackngEnemy)
        {
            if (distanceToPLayer <= _attackingDistance)
            {
                newState = State.Attacking;
            }
        }

        if (newState != _CurrentState)
        {
            if (newState == State.Chasing)
            {
                _navMeshAgent.ResetPath();
                _navMeshAgent.speed = _chasingSpeed;
            }
            else if (newState == State.Roaming)
            {
                _navMeshAgent.speed = _roamingSpeed;
                _roamingTimer = 0f;
            }

            else if (newState == State.Attacking)
            {
                _navMeshAgent.ResetPath();
            }
            _CurrentState = newState;
        }
    }
    private void AttackingTarget()
    {
        if (Time.time > _nextAttackTime)
        {
            OnEnemyAttack?.Invoke(this, EventArgs.Empty);
            _nextAttackTime = Time.time + _attackRate;
        }
    }
    public bool IsAgentRunning()
    {
        if (_navMeshAgent.velocity == Vector3.zero)
        {
            return false;
        }
        else { return true; }
        }
    private void Roaming()
    {
        _startingPosition = transform.position;
        _roamPosition = GetRoamingPosition();
        ChangeFacingDirection(_startingPosition, _roamPosition);
        _navMeshAgent.SetDestination(_roamPosition);
    }
    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + Utils.Utils.GetRandomDir() * UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);
    }
    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else { transform.rotation = Quaternion.Euler(0, 0, 0); }
    }
}

