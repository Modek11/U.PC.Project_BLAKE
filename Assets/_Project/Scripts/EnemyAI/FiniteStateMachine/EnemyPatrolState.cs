using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyBaseState
{
    private float _walkSpeed = 5f;
    private int _waypointIndex = 0;

    private EnemyFOV _enemyFOV;

    public EnemyPatrolState(NavMeshAgent navMeshAgent, GameObject playerRef, GameObject enemyRef) 
        : base(navMeshAgent, playerRef, enemyRef) 
    {
        
        _enemyFOV = enemyRef.GetComponent<EnemyFOV>();
    }

    public override void EnterState(EnemyAIManager enemy)
    {
        navMeshAgent.speed = _walkSpeed;
    }

    public override void UpdateState(EnemyAIManager enemy)
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
