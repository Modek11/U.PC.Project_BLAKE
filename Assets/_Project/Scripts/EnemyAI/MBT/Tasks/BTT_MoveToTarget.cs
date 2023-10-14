using MBT;
using System;
using UnityEngine;
using UnityEngine.AI;

[AddComponentMenu("")]
[MBTNode(name = "Tasks/Move To Target")]
public class BTT_MoveToTarget : Leaf
{
    public float AcceptableDistance = 1f;
    public float MoveSpeed = 5f;
    public GameObject Target;

    public EnemyAIManager AIController;

    private void Awake()
    {
        AcceptableDistance = MathF.Max(AcceptableDistance, 1f);
    }

    private void Start()
    {
        if(Target == null)
        {
            Target = ReferenceManager.BlakeHeroCharacter.gameObject;
        }
    }

    public override void OnEnter()
    {
        if (AIController == null) return;
        if (AIController.navMeshAgent.isStopped) AIController.navMeshAgent.isStopped = false;

        AIController.navMeshAgent.speed = MoveSpeed;
        AIController.navMeshAgent.SetDestination(Target.transform.position);
    }

    public override NodeResult Execute()
    {
        if (AIController.navMeshAgent.pathStatus == NavMeshPathStatus.PathPartial) { return NodeResult.failure; }
        if (AIController.navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid) { return NodeResult.failure; }
        if (!AIController.navMeshAgent.hasPath) { return NodeResult.failure; }
        if (AIController.navMeshAgent.velocity.sqrMagnitude == 0f) { return NodeResult.failure; }

        AIController.navMeshAgent.SetDestination(Target.transform.position);

        if (AIController.navMeshAgent.remainingDistance <= AcceptableDistance)
        {
            AIController.navMeshAgent.ResetPath();
            return NodeResult.success;
        }
        return NodeResult.running;
    }
}
