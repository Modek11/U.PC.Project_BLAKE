using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EnemyBaseState
{
    private float _weaponRange = 10f;

    private float _timeToAttack;
    private IWeapon _weaponReference;
    private IAttack _enemyAttack;

    public EnemyAttackState(NavMeshAgent navMeshAgent, GameObject playerRef, GameObject enemyRef) 
        : base(navMeshAgent, playerRef, enemyRef) 
    {
        _weaponReference = enemyRef.transform.GetComponentInChildren<Weapon>();
        _timeToAttack = 5;
    }

    public override void EnterState(EnemyAIManager enemy)
    {
        Debug.Log("SWITCHED TO ATTACK STATE");
        
        //navMeshAgent.isStopped = true;
    }

    public override void UpdateState(EnemyAIManager enemy)
    {
        float distanceToPlayer = Vector3.Distance(enemyRef.transform.position, playerRef.transform.position);
        _timeToAttack += Time.deltaTime;

        if (distanceToPlayer <= _weaponRange * 0.75f)
        {
            navMeshAgent.isStopped = true;
        }

        if (distanceToPlayer > _weaponRange)
        {
            enemy.SwitchCurrentState(enemy.ChaseState);
        }
        else
        {
            if (_timeToAttack > 5f) //wartoœæ zast¹piæ fireRate'm broni
            {
                _weaponReference.PrimaryAttack();
                Debug.Log("Shots fired");
                _timeToAttack = 0;
            }
        }
    }
}
