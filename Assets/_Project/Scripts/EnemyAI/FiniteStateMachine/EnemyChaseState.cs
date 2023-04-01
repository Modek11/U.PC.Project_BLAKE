using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyBaseState
{
    private float _weaponRange = 10f;
    private float _chaseSpeed = 12f;

    public EnemyChaseState(NavMeshAgent navMeshAgent, GameObject playerRef, GameObject enemyRef)
        : base(navMeshAgent, playerRef, enemyRef) { }


    public override void EnterState(EnemyAIManager enemy)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.ResetPath();
        navMeshAgent.speed = _chaseSpeed;
        Debug.Log("CHASE STATE");
    }

    public override void UpdateState(EnemyAIManager enemy)
    {
        float distanceToPlayer = Vector3.Distance(enemyRef.transform.position, playerRef.transform.position);

        if ((distanceToPlayer > _weaponRange && distanceToPlayer <= 2f * _weaponRange) 
            || distanceToPlayer > 3f * _weaponRange || !enemy.GetStrafeBool())
        {
            navMeshAgent.SetDestination(playerRef.transform.position);
        }

        if (distanceToPlayer > 2f * _weaponRange && distanceToPlayer <= 3f * _weaponRange && enemy.GetStrafeBool())
        {
            enemy.SwitchCurrentState(enemy.StrafeState);
        }

        if (distanceToPlayer <= _weaponRange)
        {
            enemy.SwitchCurrentState(enemy.AttackState);
        }
    }
}
