using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : EnemyBaseState
{
    private float _weaponRange;
    private float _chaseSpeed;

    private Vector3 targetPositionOffset;

    public EnemyChaseState(NavMeshAgent navMeshAgent, EnemyAIManager aIManager)
        : base(navMeshAgent, aIManager) 
    {
        _weaponRange = aiManager.GetWeaponRef().GetComponent<Weapon>().Range; 
        _chaseSpeed = 12f;
    }


    public override void EnterState()
    {
        navMeshAgent.isStopped = false;

        navMeshAgent.ResetPath();
        navMeshAgent.speed = _chaseSpeed;
        targetPositionOffset = GetTargetPositionOffset();

        Debug.Log("CHASE STATE");
    }

    public override void UpdateState()
    {
        if (aiManager.GetPlayerRef() == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(aiManager.GetEnemyRef().transform.position, aiManager.GetPlayerRef().transform.position);

        navMeshAgent.SetDestination(aiManager.GetPlayerRef().transform.position + targetPositionOffset);

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
            aiManager.SwitchCurrentState(aiManager.AttackState);
        }
    }

    private Vector3 GetTargetPositionOffset()
    {
        Vector3 offset = Random.insideUnitSphere * 5f;
        offset = new Vector3(offset.x, 0, offset.z);

        return offset;
    }
}
