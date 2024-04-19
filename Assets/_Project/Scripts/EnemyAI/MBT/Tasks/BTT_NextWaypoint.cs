using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Tasks/Next Waypoint")]
public class BTT_NextWaypoint : Leaf
{
    public AIController AIController;
    private int waypointIndex = 0;

    public Vector3Reference Vector3Reference = new Vector3Reference();

    public override NodeResult Execute()
    {
        if (AIController.Waypoints == null) return NodeResult.failure;
        if (AIController.NavMeshAgent == null) return NodeResult.failure;

        int waypointCount = AIController.Waypoints.GetCount();
        if (waypointIndex < waypointCount)
        {
            Vector3 patrolPosition = AIController.Waypoints.GetWaypointPosition(waypointIndex % waypointCount);
            if (AIController.NavMeshAgent.remainingDistance <= AIController.NavMeshAgent.stoppingDistance)
            {
                Vector3Reference.Value = patrolPosition;
                waypointIndex = (waypointIndex + 1) % waypointCount;
            }
        }
        return NodeResult.success;
    }
}
