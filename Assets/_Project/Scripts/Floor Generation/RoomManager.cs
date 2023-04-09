using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private Room activeRoom;
    [SerializeField]
    private Transform minimapFloor;

    public void SetActiveRoom(Room newRoom)
    {
        activeRoom = newRoom;
    }

    public Room GetActiveRoom()
    {
        return activeRoom;
    }

    public Transform GetMinimapFloor()
    {
        return minimapFloor;
    }
}
