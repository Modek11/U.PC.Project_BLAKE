using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyBaseState
{
    private float _walkSpeed;
    private int _waypointIndex;

    private EnemyFOV _enemyFOV;

    public EnemyPatrolState(NavMeshAgent navMeshAgent, GameObject playerRef, GameObject enemyRef, GameObject weaponRef) 
        : base(navMeshAgent, playerRef, enemyRef, weaponRef) 
    {
        _enemyFOV = enemyRef.GetComponent<EnemyFOV>();
        _walkSpeed = 5f;
        _waypointIndex = 0;
    }

    public override void EnterState(EnemyAIManager enemy)
    {
        navMeshAgent.speed = _walkSpeed;
    }

    public override void UpdateState(EnemyAIManager enemy)
    {
        if (enemy.waypoints != null)
        {
            if (_waypointIndex < enemy.waypoints.GetCount())
            {
                Vector3 patrolPosition = enemy.waypoints.GetWaypointPosition(_waypointIndex);
                MoveToPosition(patrolPosition);

                if (Vector3.Distance(enemyRef.transform.position, patrolPosition) < .1f)
                {
                    _waypointIndex++;
                }

            }
            else
            {
                _waypointIndex = 0;
            }
        }

        if (_enemyFOV.canSeePlayer)
        {
            enemy.SwitchCurrentState(enemy.ChaseState);
        }
    }

    private void MoveToPosition(Vector3 movePosition)
    {
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            navMeshAgent.SetDestination(movePosition);
        }
    }
}
