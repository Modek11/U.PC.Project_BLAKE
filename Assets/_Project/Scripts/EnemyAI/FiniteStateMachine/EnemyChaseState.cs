using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyBaseState
{
    private float _weaponRange = 10f;
    private float _chaseSpeed = 8f;

    public EnemyChaseState(NavMeshAgent navMeshAgent, GameObject playerRef, GameObject enemyRef) 
        : base(navMeshAgent, playerRef, enemyRef) { }

    public override void EnterState(EnemyAIManager enemy)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.ResetPath();
        navMeshAgent.speed = _chaseSpeed;
    }

    public override void UpdateState(EnemyAIManager enemy)
    {
        navMeshAgent.SetDestination(playerRef.transform.position);

        if (Vector3.Distance(enemyRef.transform.position, playerRef.transform.position) <= _weaponRange)
        {
            enemy.SwitchCurrentState(enemy.AttackState);
        }
    }

}
