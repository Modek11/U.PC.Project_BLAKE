using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EnemyBaseState
{
    private float _weaponRange;

    private float _timeToAttack;
    private float _attackDelay;

    private IWeapon _weaponInterface;
    private Weapon _weaponScript;

    private EnemyFOV _enemyFOV;
    private Vector3 _targetPositionOffset;


    public EnemyAttackState(NavMeshAgent navMeshAgent, EnemyAIManager aIManager) 
        : base(navMeshAgent, aIManager) 
    {
        _weaponScript = aiManager.GetWeaponRef().GetComponent<Weapon>();
        _weaponInterface = _weaponScript;

        _weaponRange = _weaponScript.Range;

        _attackDelay = _weaponScript.GetCurrentWeaponFireRate();
        _timeToAttack = 0.5f * _attackDelay;

        _enemyFOV = aiManager.GetEnemyRef().GetComponent<EnemyFOV>();
    }

    public override void EnterState()
    {
        Debug.Log("SWITCHED TO ATTACK STATE");
        _targetPositionOffset = GetTargetPositionOffset();
    }

    public override void UpdateState()
    {
        if (aiManager.GetPlayerRef() == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(aiManager.GetEnemyRef().transform.position, aiManager.GetPlayerRef().transform.position);
        _timeToAttack += Time.deltaTime;

        navMeshAgent.SetDestination(aiManager.GetPlayerRef().transform.position + _targetPositionOffset);

        if (distanceToPlayer <= _weaponRange * 0.3f)
        {
            navMeshAgent.isStopped = true;
        }

        if (distanceToPlayer > _weaponRange)
        {
            aiManager.SwitchCurrentState(aiManager.ChaseState);
        }
        else
        {
            if (_timeToAttack > _attackDelay && _enemyFOV.canSeePlayer)
            {
                _weaponInterface.PrimaryAttack();
                Debug.Log("Shots fired");
                _timeToAttack = 0;
            }
        }
    }

    private Vector3 GetTargetPositionOffset()
    {
        Vector3 offset = Random.insideUnitSphere * 5f;
        offset = new Vector3(offset.x, 0, offset.z);

        return offset;
    }
}
