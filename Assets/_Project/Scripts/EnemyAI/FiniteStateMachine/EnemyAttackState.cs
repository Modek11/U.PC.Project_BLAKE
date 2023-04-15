using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EnemyBaseState
{
    private float _weaponRange;

    private float _timeToAttack;
    private float _attackDelay;

    private IWeapon _weaponInterface;
    private Weapon _weaponScript;


    public EnemyAttackState(NavMeshAgent navMeshAgent, GameObject playerRef, GameObject enemyRef, GameObject weaponRef) 
        : base(navMeshAgent, playerRef, enemyRef, weaponRef) 
    {
        _weaponScript = weaponRef.GetComponent<Weapon>();
        _weaponInterface = _weaponScript;

        _weaponRange = 3f * _weaponScript.Range;

        _attackDelay = _weaponScript.GetCurrentWeaponFireRate();
        _timeToAttack = _attackDelay;
    }

    public override void EnterState(EnemyAIManager enemy)
    {
        Debug.Log("SWITCHED TO ATTACK STATE");
    }

    public override void UpdateState(EnemyAIManager enemy)
    {
        if (playerRef == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(enemyRef.transform.position, playerRef.transform.position);
        _timeToAttack += Time.deltaTime;

        if (distanceToPlayer <= _weaponRange * 0.6f)
        {
            navMeshAgent.isStopped = true;
        }

        if (distanceToPlayer > _weaponRange)
        {
            enemy.SwitchCurrentState(enemy.ChaseState);
        }
        else
        {
            if (_timeToAttack > _attackDelay) //warto�� zast�pi� fireRate'm broni
            {
                _weaponInterface.PrimaryAttack();
                Debug.Log("Shots fired");
                _timeToAttack = 0;
            }
        }
    }
}
