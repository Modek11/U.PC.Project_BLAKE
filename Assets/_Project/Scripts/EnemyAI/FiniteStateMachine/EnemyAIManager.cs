using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIManager : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private GameObject _playerRef;
    private GameObject _enemyRef;
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
    }

    private void OnEnable()
    {
        PatrolState = new EnemyPatrolState(_navMeshAgent, _playerRef, _enemyRef, weaponRef);
        ChaseState = new EnemyChaseState(_navMeshAgent, _playerRef, _enemyRef, weaponRef);
        AttackState = new EnemyAttackState(_navMeshAgent, _playerRef, _enemyRef, weaponRef);
        StrafeState = new EnemyStrafeState(_navMeshAgent, _playerRef, _enemyRef, weaponRef);
    }

    private void Start()
    {
        _currentState = PatrolState;
        _currentState.EnterState(this);
    }

    private void Update()
    {
        _currentState.UpdateState(this);
        FaceThePlayer();
    }

    public void SwitchCurrentState(EnemyBaseState state)
    {
        _currentState = state;
        _currentState.EnterState(this);
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
}
