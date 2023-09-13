using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
	[SerializeField] 
	private List<Transform> waypoints;

	private int _waypointsCount;

	private void Start()
	{
		_waypointsCount = waypoints.Count;
	}

	public Vector3 GetWaypointPosition(int waypointIndex)
	{
		return waypoints[waypointIndex].position;
	}

	public int GetCount()
	{
		return _waypointsCount;
	}
}
