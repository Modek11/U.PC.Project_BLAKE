using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStrafeState : EnemyBaseState
{
    private float _weaponRange = 10f;

    private Transform strafeRight;
    private Transform strafeLeft;
    bool strafeToRight;

    public EnemyStrafeState(NavMeshAgent navMeshAgent, GameObject playerRef, GameObject enemyRef)
        : base(navMeshAgent, playerRef, enemyRef) 
    {
        strafeRight = enemyRef.transform.Find("StrafeRight");
        strafeLeft = enemyRef.transform.Find("StrafeLeft");
        strafeToRight = Random.value > 0.5f ? true : false;
    }

    public override void EnterState(EnemyAIManager enemy)
    {
        Debug.Log("STRAFE STATE");
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = 8f;
        navMeshAgent.ResetPath();

        if (strafeToRight)
        {
            navMeshAgent.SetDestination(strafeRight.position);
        }
        else
        {
            navMeshAgent.SetDestination(strafeLeft.position);
        }
        strafeToRight = !strafeToRight;
    }

    public override void UpdateState(EnemyAIManager enemy)
    {
        float distanceToPlayer = Vector3.Distance(enemyRef.transform.position, playerRef.transform.position);

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            enemy.SwitchCurrentState(enemy.StrafeState);
            Debug.Log("STRAFING");
        }

        if (distanceToPlayer <= _weaponRange)
        {
            enemy.SwitchCurrentState(enemy.AttackState);
        }

        if (distanceToPlayer < 2f * _weaponRange || distanceToPlayer > 3f * _weaponRange)
        {
            enemy.SwitchCurrentState(enemy.ChaseState);
        }
    }
}
