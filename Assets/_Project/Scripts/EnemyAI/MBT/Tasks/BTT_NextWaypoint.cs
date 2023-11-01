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
        if (AIController.waypoints == null) return NodeResult.failure;

        int waypointCount = AIController.waypoints.GetCount();
        if (waypointIndex < waypointCount)
        {
            Vector3 patrolPosition = AIController.waypoints.GetWaypointPosition(waypointIndex % waypointCount);
            if (AIController.navMeshAgent.remainingDistance <= AIController.navMeshAgent.stoppingDistance)
            {
                Vector3Reference.Value = patrolPosition;
                waypointIndex = (waypointIndex + 1) % waypointCount;
            }
        }
        return NodeResult.success;
    }
}
