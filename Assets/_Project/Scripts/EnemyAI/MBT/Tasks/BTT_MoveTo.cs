using MBT;
using System;
using UnityEngine;
using UnityEngine.AI;

[AddComponentMenu("")]
[MBTNode(name = "Tasks/Move To")]
public class BTT_MoveTo : Leaf
{
    public float AcceptableDistance = 1f;
    public Vector3Reference LocationReference = new Vector3Reference();
    public bool isPatrolling = false;

    private AIController aiController;

    private void Awake()
    {
        AcceptableDistance = MathF.Max(AcceptableDistance, 1f);
    }

    public override void OnEnter()
    {
        aiController = GetComponent<AIController>();
        if (aiController == null) return;
        if (aiController.NavMeshAgent.isStopped) aiController.NavMeshAgent.isStopped = false;

        float speed = (!isPatrolling) ? aiController.GetEnemyScript().CalculateSpeed() : aiController.GetEnemyScript().GetPatrolSpeed();
        aiController.NavMeshAgent.speed = speed;
        aiController.NavMeshAgent.SetDestination(LocationReference.Value);
    }

    public override NodeResult Execute()
    {
        if (aiController.NavMeshAgent.pathStatus == NavMeshPathStatus.PathPartial) { return NodeResult.failure; }
        if (aiController.NavMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid) { return NodeResult.failure; }
        if (!aiController.NavMeshAgent.hasPath) { return NodeResult.failure; }
        if (aiController.NavMeshAgent.velocity.sqrMagnitude == 0f) { return NodeResult.failure; }
        
        float speed = (!isPatrolling) ? aiController.GetEnemyScript().CalculateSpeed() : aiController.GetEnemyScript().GetPatrolSpeed();
        aiController.NavMeshAgent.speed = speed;

        if (aiController.NavMeshAgent.remainingDistance <= AcceptableDistance)
        {
            aiController.NavMeshAgent.ResetPath();
            return NodeResult.success;
        }
        return NodeResult.running;
    }
}
