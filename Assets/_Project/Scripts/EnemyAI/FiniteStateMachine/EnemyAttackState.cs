using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EnemyBaseState
{
    private float _weaponRange;

    private float _timeToAttack;
    private float _attackDelay;

    private IWeapon _weaponInterface;
    private Weapon _weaponScript;


    public EnemyAttackState(NavMeshAgent navMeshAgent, EnemyAIManager aIManager) 
        : base(navMeshAgent, aIManager) 
    {
        _weaponScript = aiManager.GetWeaponRef().GetComponent<Weapon>();
        _weaponInterface = _weaponScript;

        _weaponRange = 3f * _weaponScript.Range;

        _attackDelay = _weaponScript.GetCurrentWeaponFireRate();
        _timeToAttack = _attackDelay;
    }

    public override void EnterState()
    {
        Debug.Log("SWITCHED TO ATTACK STATE");
        navMeshAgent.SetDestination(aiManager.GetPlayerRef().transform.position + Random.insideUnitSphere * _weaponRange);
    }

    public override void UpdateState()
    {
        if (aiManager.GetPlayerRef() == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(aiManager.GetEnemyRef().transform.position, aiManager.GetPlayerRef().transform.position);
        _timeToAttack += Time.deltaTime;

        if (distanceToPlayer <= _weaponRange * 0.6f)
        {
            navMeshAgent.isStopped = true;
        }

        if (distanceToPlayer > _weaponRange)
        {
            aiManager.SwitchCurrentState(aiManager.ChaseState);
        }
        else
        {
            if (_timeToAttack > _attackDelay) //wartoœæ zast¹piæ fireRate'm broni
            {
                _weaponInterface.PrimaryAttack();
                Debug.Log("Shots fired");
                _timeToAttack = 0;
            }
        }
    }
}
