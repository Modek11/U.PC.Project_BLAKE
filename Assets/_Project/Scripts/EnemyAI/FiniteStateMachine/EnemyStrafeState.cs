using UnityEngine;
using UnityEngine.AI;

public class EnemyStrafeState : EnemyBaseState
{
    private float _weaponRange;
    private float _strafeSpeed;

    private Transform strafeRight;
    private Transform strafeLeft;
    private bool strafeToRight;

    public EnemyStrafeState(NavMeshAgent navMeshAgent, EnemyAIManager aIManager)
        : base(navMeshAgent, aIManager) 
    {
        strafeRight = aiManager.GetEnemyRef().transform.Find("StrafeRight");
        strafeLeft = aiManager.transform.Find("StrafeLeft");
        strafeToRight = Random.value > 0.5f ? true : false;

        _weaponRange = 3f * aiManager.GetWeaponRef().GetComponent<Weapon>().Range;
        _strafeSpeed = 8f;
    }

    public override void EnterState()
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

    public override void UpdateState()
    {
        if (aiManager.GetPlayerRef() == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(aiManager.GetEnemyRef().transform.position, aiManager.GetPlayerRef().transform.position);

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            aiManager.SwitchCurrentState(aiManager.StrafeState);
            Debug.Log("STRAFING");
        }

        if (distanceToPlayer <= _weaponRange)
        {
            aiManager.SwitchCurrentState(aiManager.AttackState);
        }

        if (distanceToPlayer < 1.5f * _weaponRange || distanceToPlayer > 2.5f * _weaponRange)
        {
            aiManager.SwitchCurrentState(aiManager.ChaseState);
        }
    }
}
