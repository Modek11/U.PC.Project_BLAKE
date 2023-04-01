using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : EnemyBaseState
{
    private float _weaponRange = 10f;
    private float _timeToAttack;

    public EnemyAttackState(NavMeshAgent navMeshAgent, GameObject playerRef, GameObject enemyRef) 
        : base(navMeshAgent, playerRef, enemyRef) { }

    public override void EnterState(EnemyAIManager enemy)
    {
        Debug.Log("SWITCHED TO ATTACK STATE");
        //navMeshAgent.isStopped = true;
    }

    public override void UpdateState(EnemyAIManager enemy)
    {
        float distanceToPlayer = Vector3.Distance(enemyRef.transform.position, playerRef.transform.position);
        _timeToAttack += Time.time;

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

            if (_timeToAttack >= 3f) //wartoœæ zast¹piæ fireRate'm broni
            {
                Debug.Log("ATTACK!");
                _timeToAttack = 0;
            }
        }
    }
}
