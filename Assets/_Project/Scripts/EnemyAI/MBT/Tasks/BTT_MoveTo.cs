using MBT;
using System;
using UnityEngine;
using UnityEngine.AI;

[AddComponentMenu("")]
[MBTNode(name = "Tasks/Move To")]
public class BTT_MoveTo : Leaf
{
    public float AcceptableDistance = 1f;
    public float MoveSpeed = 5f;
    public Vector3Reference LocationReference = new Vector3Reference();

    public AIController AIController;

    private void Awake()
    {
        AcceptableDistance = MathF.Max(AcceptableDistance, 1f);
    }

    public override void OnEnter()
    {
        if (AIController == null) return;
        if (AIController.navMeshAgent.isStopped) AIController.navMeshAgent.isStopped = false;

        AIController.navMeshAgent.speed = MoveSpeed;
        AIController.navMeshAgent.SetDestination(LocationReference.Value);
    }

    public override NodeResult Execute()
    {
        if (AIController.navMeshAgent.pathStatus == NavMeshPathStatus.PathPartial) { return NodeResult.failure; }
        if (AIController.navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid) { return NodeResult.failure; }
        if (!AIController.navMeshAgent.hasPath) { return NodeResult.failure; }
        if (AIController.navMeshAgent.velocity.sqrMagnitude == 0f) { return NodeResult.failure; }

        if (AIController.navMeshAgent.remainingDistance <= AcceptableDistance)
        {
            AIController.navMeshAgent.ResetPath();
            return NodeResult.success;
        }
        return NodeResult.running;
    }
}
