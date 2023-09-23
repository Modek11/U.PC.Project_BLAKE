using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Tasks/Next Waypoint")]
public class BTT_NextWaypoint : Leaf
{
    public EnemyAIManager enemyAIManager;
    private int _waypointIndex = 0;

    public Vector3Reference vector3Reference = new Vector3Reference();

    public override NodeResult Execute()
    {
        if (enemyAIManager.waypoints == null) return NodeResult.failure;

        int waypointCount = enemyAIManager.waypoints.GetCount();
        if (_waypointIndex < enemyAIManager.waypoints.GetCount())
        {
            Vector3 patrolPosition = enemyAIManager.waypoints.GetWaypointPosition(_waypointIndex % waypointCount);
            if (enemyAIManager.navMeshAgent.remainingDistance <= enemyAIManager.navMeshAgent.stoppingDistance)
            {
                vector3Reference.Value = patrolPosition;
                _waypointIndex = (_waypointIndex + 1) % waypointCount;
            }
        }
        return NodeResult.success;
    }
}
