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
        if (Vector3.Distance(enemyRef.transform.position, playerRef.transform.position) <= _weaponRange / 2f)
        {
            navMeshAgent.isStopped = true;
        }

        if (Vector3.Distance(enemyRef.transform.position, playerRef.transform.position) > _weaponRange)
        {
            enemy.SwitchCurrentState(enemy.ChaseState);
        }
        else
        {
            _timeToAttack += Time.time;

            if(_timeToAttack >= 1f)
            {
                Debug.Log("ATTACK!");
                _timeToAttack = 0;
            }
        }
    }
}
