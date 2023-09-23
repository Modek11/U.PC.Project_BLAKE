using MBT;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static EnemyFOV;

[RequireComponent(typeof(BlakeCharacter))]
public class EnemyAIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject weaponRef;

    [HideInInspector]
    public NavMeshAgent navMeshAgent;

    private GameObject _playerRef;
    private GameObject _enemyRef;
    private Animator _animator;

    private EnemyBaseState _currentState;

    public CombatStateReference combatStateReference;
    public BoolReference hasLineOfSightReference;

    public EnemyPatrolState PatrolState;
    public EnemyChaseState ChaseState;
    public EnemyAttackState AttackState;
    public EnemyStrafeState StrafeState;

    public Waypoints waypoints;


    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        _playerRef = GameObject.FindGameObjectWithTag("Player");
        _enemyRef = gameObject;
        _animator = GetComponent<Animator>();
        GetComponent<BlakeCharacter>().onDeath += Die;
        GetComponent<EnemyFOV>().onCanSeePlayerChanged += OnCanSeePlayerChanged;
    }

    private void Die()
    {
        this.enabled = false;
        navMeshAgent.isStopped = true;
        GetComponent<MBTExecutor>().enabled = false;
        //_navMeshAgent.enabled = false;
    }

    private void OnEnable()
    {
        PatrolState = new EnemyPatrolState(navMeshAgent, this);

        ChaseState = new EnemyChaseState(navMeshAgent, this);

        AttackState = new EnemyAttackState(navMeshAgent, this);

        //StrafeState = new EnemyStrafeState(_navMeshAgent, this);

    }

    private void Start()
    {
        //_currentState = PatrolState;
        //_currentState.EnterState();
    }

    private void Update()
    {
        //_currentState.UpdateState();
        FaceThePlayer();

        _animator.SetFloat("Direction", BlakeAnimatorHelper.CalculateDirection(navMeshAgent.velocity, transform));
        _animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
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

        if (combatStateReference.GetVariable().Value != CombatState.Patrol)
        {
            Vector3 direction = (_playerRef.transform.position - _enemyRef.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            
            _enemyRef.transform.rotation = Quaternion.Slerp(_enemyRef.transform.rotation, lookRotation, Time.deltaTime * 40f);
        }
    }

    private void OnCanSeePlayerChanged(bool newCanSeePlayer)
    {
        hasLineOfSightReference.Value = newCanSeePlayer;
        if (newCanSeePlayer)
        {
            combatStateReference.GetVariable().Value = CombatState.Chase;
            if (IsInvoking("ClearPlayerFocus"))
            {
                CancelInvoke("ClearPlayerFocus");
            }
        }
        else
        {
            Invoke("ClearPlayerFocus", 6f);
        }
    }

    private void ClearPlayerFocus()
    {
        combatStateReference.GetVariable().Value = CombatState.Patrol;
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
