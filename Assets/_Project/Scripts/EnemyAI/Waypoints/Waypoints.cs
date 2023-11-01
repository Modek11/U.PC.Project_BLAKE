using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
	[SerializeField] 
	private List<Transform> waypoints;

	private int waypointsCount;

	private void Start()
	{
		waypointsCount = waypoints.Count;
	}

	public Vector3 GetWaypointPosition(int waypointIndex)
	{
		return waypoints[waypointIndex].position;
	}

	public int GetCount()
	{
		return waypointsCount;
	}
}
