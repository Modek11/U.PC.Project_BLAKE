using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyBaseState
{
    private float _walkSpeed;
    private int _waypointIndex;

    private EnemyFOV _enemyFOV;

    public EnemyPatrolState(NavMeshAgent navMeshAgent, EnemyAIManager aIManager) 
        : base(navMeshAgent, aIManager) 
    {
        _enemyFOV = aiManager.GetEnemyRef().GetComponent<EnemyFOV>();
        _walkSpeed = 5f;
        _waypointIndex = 0;
    }

    public override void EnterState()
    {
        navMeshAgent.speed = _walkSpeed;
        Debug.Log("Entering Patrol");
    }

    public override void UpdateState()
    {
        if (aiManager.waypoints != null)
        {
            if (_waypointIndex < aiManager.waypoints.GetCount())
            {
                Vector3 patrolPosition = aiManager.waypoints.GetWaypointPosition(_waypointIndex);
                MoveToPosition(patrolPosition);

                if (Vector3.Distance(aiManager.transform.position, patrolPosition) < .1f)
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
            aiManager.SwitchCurrentState(aiManager.ChaseState);
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
