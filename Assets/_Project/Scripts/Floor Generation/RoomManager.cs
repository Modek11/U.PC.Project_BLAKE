using System.Collections;
using System.Collections.Generic;
using _Project.Scripts;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private Room activeRoom;
    private Room previousRoom;

    [SerializeField]
    private Transform minimapFloor;

    private void Awake()
    {
        ReferenceManager.RoomManager = this;
    }

    public void SetActiveRoom(Room newRoom)
    {
        activeRoom = newRoom;
    }

    public Room GetActiveRoom()
    {
        return activeRoom;
    }

    public void SetPreviousRoom(Room newRoom)
    {
        previousRoom = newRoom;
    }

    public Room GetPreviousRoom()
    {
        return previousRoom;
    }

    public Transform GetMinimapFloor()
    {
        return minimapFloor;
    }
}
