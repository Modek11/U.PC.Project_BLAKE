using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPathfinding
{
    private List<GameObject> floor;
    private List<Room> openList;
    private List<Room> closedList;

    public FloorPathfinding(List<GameObject> floor)
    {
        this.floor = floor;
    }

    public List<Room> FindPath(GameObject startRoomObject, GameObject endRoomObject)
    {
        Room startRoom = startRoomObject.GetComponent<Room>();
        Room endRoom = endRoomObject.GetComponent<Room>();
        openList = new List<Room> { startRoom };
        closedList = new List<Room>();
        Debug.Log("End Room: " + endRoomObject.name);
        for(int i =0; i < floor.Count; i++)
        {
            Room room = floor[i].GetComponent<Room>();
            room.gCost = int.MaxValue;
            room.CalculateFCost();
            room.cameFromRoom = null;
        }
        startRoom.gCost = 0;
        startRoom.hCost = CalculateDistanceCost(startRoom, endRoom);
        startRoom.CalculateFCost();

        while(openList.Count>0)
        {
            Room currentRoom = GetLowestFCostNode(openList);
            if(currentRoom == endRoom)
            {
                return CalculatePath(endRoom);
            }

            openList.Remove(currentRoom);
            closedList.Add(currentRoom);
            List<Room> neighbourList = GetNeighbourList(currentRoom);
            foreach(Room neighbourRoom in neighbourList)
            {
                if (closedList.Contains(neighbourRoom)) continue;

                int tentativeGCost = currentRoom.gCost + CalculateDistanceCost(currentRoom, neighbourRoom);
                if(tentativeGCost < neighbourRoom.gCost)
                {
                    neighbourRoom.cameFromRoom = currentRoom;
                    neighbourRoom.gCost = tentativeGCost;
                    neighbourRoom.hCost = CalculateDistanceCost(neighbourRoom, endRoom);
                    neighbourRoom.CalculateFCost();

                    if(!openList.Contains(neighbourRoom))
                    {
                        openList.Add(neighbourRoom);
                    }
                }
            }
        }
        return null;
    }

    private List<Room> GetNeighbourList(Room room)
    {
        List<Room> neighbours = new List<Room>();
        foreach(RoomConnector door in room.GetDoors())
        {
            if(door.GetConnector() != null)
            {
                if (door.GetConnector().GetRoom() != null)
                {
                    neighbours.Add(door.GetConnector().GetRoom());
                }
            }
        }
        return neighbours;
    }

    private List<Room> CalculatePath(Room endRoom)
    {
        List<Room> path = new List<Room>();
        path.Add(endRoom);
        Room currentRoom = endRoom;
        while(currentRoom.cameFromRoom != null)
        {
            path.Add(currentRoom.cameFromRoom);
            currentRoom = currentRoom.cameFromRoom;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(Room a, Room b)
    {
        int xDistance = (int)Mathf.Abs(a.gameObject.transform.position.x - b.gameObject.transform.position.x);
        int yDistance = (int)Mathf.Abs(a.gameObject.transform.position.y - b.gameObject.transform.position.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return Mathf.Min(xDistance, yDistance);
    }

    private Room GetLowestFCostNode(List<Room> roomObjectList)
    {
        Room lowestFCostRoom = roomObjectList[0];
        for(int i = 1; i<roomObjectList.Count; i++)
        {
            if (roomObjectList[i].fCost < lowestFCostRoom.fCost)
            {
                lowestFCostRoom = roomObjectList[i];
            }
        }
        return lowestFCostRoom;
    }

}
