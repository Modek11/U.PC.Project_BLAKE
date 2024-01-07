using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConnector : MonoBehaviour
{
    [SerializeField]
    private RoomConnector connectedDoor;

    [SerializeField]
    private GameObject doorObject;

    [SerializeField]
    private GameObject wallObject;

    [SerializeField]
    private Door door;

    [SerializeField]
    private Room mainRoom;
    private bool isActive = false;

    public void SetConnector(RoomConnector room)
    {
        connectedDoor = room;
    }

    public void SetDoor()
    {
        if (connectedDoor != null)
        {
            doorObject.SetActive(true);
            isActive = true;
            wallObject.SetActive(false);
        }
        else
        {
            doorObject.SetActive(false);
            isActive = false;
            wallObject.SetActive(true);
        }
    }

    public void OpenDoor()
    {
        if (!isActive) return;
        door.OpenDoor();
    }

    public void CloseDoor()
    {
        if (!isActive) return;
        door.CloseDoor();
    }

    public void UnlockDoor()
    {
        if (!isActive) return;
        door.UnlockDoor();
    }

    public void LockDoor()
    {
        if (!isActive) return;
        door.LockDoor();
    }

    public RoomConnector GetConnector()
    {
        return connectedDoor;
    }


    public void SetRoom(Room room)
    {
        mainRoom = room;
    }

    public Room GetRoom()
    {
        return mainRoom;
    }
}
