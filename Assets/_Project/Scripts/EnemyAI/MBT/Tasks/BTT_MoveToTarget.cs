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

    public AIController AIController;

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
        if (AIController.NavMeshAgent.isStopped) AIController.NavMeshAgent.isStopped = false;

        AIController.NavMeshAgent.speed = MoveSpeed;
        AIController.NavMeshAgent.SetDestination(Target.transform.position);
    }

    public override NodeResult Execute()
    {
        if (AIController.NavMeshAgent.pathStatus == NavMeshPathStatus.PathPartial) { return NodeResult.failure; }
        if (AIController.NavMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid) { return NodeResult.failure; }
        if (!AIController.NavMeshAgent.hasPath) { return NodeResult.failure; }
        if (AIController.NavMeshAgent.velocity.sqrMagnitude == 0f) { return NodeResult.failure; }

        AIController.NavMeshAgent.SetDestination(Target.transform.position);

        if (AIController.NavMeshAgent.remainingDistance <= AcceptableDistance)
        {
            AIController.NavMeshAgent.ResetPath();
            return NodeResult.success;
        }
        return NodeResult.running;
    }
}
