using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private Room activeRoom;

    public void SetActiveRoom(Room newRoom)
    {
        activeRoom = newRoom;
    }

    public Room GetActiveRoom()
    {
        return activeRoom;
    }
}
