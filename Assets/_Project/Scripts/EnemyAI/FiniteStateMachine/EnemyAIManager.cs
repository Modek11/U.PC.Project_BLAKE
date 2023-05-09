using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BlakeCharacter))]
public class EnemyAIManager : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private GameObject _playerRef;
    private GameObject _enemyRef;
    private Animator _animator;
    [SerializeField] private GameObject weaponRef;

    private EnemyBaseState _currentState;

    public EnemyPatrolState PatrolState;
    public EnemyChaseState ChaseState;
    public EnemyAttackState AttackState;
    public EnemyStrafeState StrafeState;

    public Waypoints waypoints;


    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _playerRef = GameObject.FindGameObjectWithTag("Player");
        _enemyRef = gameObject;
        _animator = GetComponent<Animator>();
        GetComponent<BlakeCharacter>().onDeath += Die;
    }

    private void Die()
    {
        this.enabled = false;
        _navMeshAgent.isStopped = true;
        //_navMeshAgent.enabled = false;
    }

    private void OnEnable()
    {
        PatrolState = new EnemyPatrolState(_navMeshAgent, this);

        ChaseState = new EnemyChaseState(_navMeshAgent, this);

        AttackState = new EnemyAttackState(_navMeshAgent, this);

        //StrafeState = new EnemyStrafeState(_navMeshAgent, this);

    }

    private void Start()
    {
        _currentState = PatrolState;
        _currentState.EnterState();

    }

    private void Update()
    {
        _currentState.UpdateState();
        FaceThePlayer();

        _animator.SetFloat("Direction", BlakeAnimatorHelper.CalculateDirection(_navMeshAgent.velocity, transform));
        _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
    }

    public void UpdatePlayerRef()
    {
        _playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    public void SwitchCurrentState(EnemyBaseState state)
    {
        _currentState = state;
        _currentState.EnterState();
    }

    public void SetWaypoints(Waypoints waypoints)
    {
        this.waypoints = waypoints;
    }

    private void FaceThePlayer()
    {
        if (_playerRef == null)
        {
            return;
        }

        Vector3 direction = (_playerRef.transform.position - _enemyRef.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

        if (_currentState != PatrolState)
        {
            _enemyRef.transform.rotation = Quaternion.Slerp(_enemyRef.transform.rotation, lookRotation, Time.deltaTime * 3f);
        }
    }

    public GameObject GetPlayerRef()
    {
        return _playerRef;
    }

    public GameObject GetEnemyRef()
    {
        return _enemyRef;
    }

    public GameObject GetWeaponRef()
    {
        return weaponRef;
    }
}
