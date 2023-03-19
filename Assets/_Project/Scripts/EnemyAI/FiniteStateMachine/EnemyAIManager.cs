using UnityEngine;
using UnityEngine.AI;

public class EnemyAIManager : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private GameObject _playerRef;
    private GameObject _enemyRef;
    private Transform _groundCenterPoint;

    public EnemyBaseState currentState;

    public EnemyPatrolState PatrolState;
    public EnemyChaseState ChaseState;
    public EnemyAttackState AttackState;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _playerRef = GameObject.FindGameObjectWithTag("Player");
        _enemyRef = gameObject;
        _groundCenterPoint = GameObject.Find("Floor").transform;
    }

    private void OnEnable()
    {
        PatrolState = new EnemyPatrolState(_navMeshAgent, _playerRef, _enemyRef, _groundCenterPoint);
        ChaseState = new EnemyChaseState(_navMeshAgent, _playerRef, _enemyRef);
        AttackState = new EnemyAttackState(_navMeshAgent, _playerRef, _enemyRef);
    }

    private void Start()
    {
        currentState = PatrolState;
        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchCurrentState(EnemyBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}
