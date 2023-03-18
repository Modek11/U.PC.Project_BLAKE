using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyBaseState
{
    private float _randomPointRange = 100.0f;
    private float _walkSpeed = 5f;

    private Transform _groundCenterPoint;

    private EnemyFOV _enemyFOV;

    public EnemyPatrolState(NavMeshAgent navMeshAgent, GameObject playerRef, GameObject enemyRef, Transform groundCenterPoint) 
        : base(navMeshAgent, playerRef, enemyRef) 
    {
        _groundCenterPoint = groundCenterPoint;
    }

    public override void EnterState(EnemyAIManager enemy)
    {
        _enemyFOV = enemyRef.GetComponent<EnemyFOV>();
        navMeshAgent.speed = _walkSpeed;
    }

    public override void UpdateState(EnemyAIManager enemy)
    {
        SetRandomPosition(_groundCenterPoint.position, _randomPointRange);

        if (_enemyFOV.canSeePlayer)
        {
            enemy.SwitchCurrentState(enemy.ChaseState);
        }
    }

    private void SetRandomPosition(Vector3 center, float range)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                MoveToPosition(hit.position);
            }
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
