using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyBaseState
{
    private float _weaponRange;
    private float _chaseSpeed;

    private Vector3 targetPositionOffset;

    public EnemyChaseState(NavMeshAgent navMeshAgent, GameObject playerRef, GameObject enemyRef, GameObject weaponRef)
        : base(navMeshAgent, playerRef, enemyRef, weaponRef) 
    {
        _weaponRange = 3f * weaponRef.GetComponent<Weapon>().Range; 
        _chaseSpeed = 12f;
    }


    public override void EnterState(EnemyAIManager enemy)
    {
        navMeshAgent.isStopped = false;

        navMeshAgent.ResetPath();
        navMeshAgent.speed = _chaseSpeed;
        targetPositionOffset = GetTargetPositionOffset();

        Debug.Log("CHASE STATE");
    }

    public override void UpdateState(EnemyAIManager enemy)
    {
        if (playerRef == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(enemyRef.transform.position, playerRef.transform.position);

        if (distanceToPlayer > _weaponRange)
        {
            navMeshAgent.SetDestination(playerRef.transform.position + targetPositionOffset);
        }

        /*
        if ((distanceToPlayer > _weaponRange && distanceToPlayer <= 1.5f * _weaponRange) 
            || distanceToPlayer > 2.5f * _weaponRange)
        {
            navMeshAgent.SetDestination(playerRef.transform.position + Random.insideUnitSphere * _weaponRange);
        }

        if (distanceToPlayer > 1.5f * _weaponRange && distanceToPlayer <= 2.5f * _weaponRange && _enemyFOV.canSeePlayer)
        {
            enemy.SwitchCurrentState(enemy.StrafeState);
        }
        */

        if (distanceToPlayer <= _weaponRange)
        {
            enemy.SwitchCurrentState(enemy.AttackState);
        }
    }

    private Vector3 GetTargetPositionOffset()
    {
        Vector3 offset = Random.insideUnitSphere * 5f;
        offset = new Vector3(offset.x, 0, offset.z);

        return offset;
    }
}
