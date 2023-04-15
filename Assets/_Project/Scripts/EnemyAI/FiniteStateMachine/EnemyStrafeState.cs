using UnityEngine;
using UnityEngine.AI;

public class EnemyStrafeState : EnemyBaseState
{
    private float _weaponRange;
    private float _strafeSpeed;

    private Transform strafeRight;
    private Transform strafeLeft;
    private bool strafeToRight;

    public EnemyStrafeState(NavMeshAgent navMeshAgent, GameObject playerRef, GameObject enemyRef, GameObject weaponRef)
        : base(navMeshAgent, playerRef, enemyRef, weaponRef) 
    {
        strafeRight = enemyRef.transform.Find("StrafeRight");
        strafeLeft = enemyRef.transform.Find("StrafeLeft");
        strafeToRight = Random.value > 0.5f ? true : false;

        _weaponRange = 3f * weaponRef.GetComponent<Weapon>().Range;
        _strafeSpeed = 8f;
    }

    public override void EnterState(EnemyAIManager enemy)
    {
        Debug.Log("STRAFE STATE");
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = _strafeSpeed;
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
        if (playerRef == null)
        {
            return;
        }

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

        if (distanceToPlayer < 1.5f * _weaponRange || distanceToPlayer > 2.5f * _weaponRange)
        {
            enemy.SwitchCurrentState(enemy.ChaseState);
        }
    }
}
