using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private RoomConnector[] doors;
    [SerializeField]
    private RandomizedRoomObject[] randomObjects;
    [SerializeField]
    private GameObject fog;
    private RoomManager roomManager;

    [Header("Minimap variables")]
    [SerializeField]
    private MinimapRoom minimapRoom;

    public void InitializeRoom(RoomManager rm)
    {
        roomManager = rm;
        foreach(RandomizedRoomObject randomObject in randomObjects)
        {
            randomObject.InitializeRandomObject();
        }
        foreach(RoomConnector door in doors)
        {
            door.SetRoom(this);
            door.SetDoor();
        }

        if(minimapRoom != null && roomManager.GetMinimapFloor() != null)
        {
            minimapRoom.transform.parent = roomManager.GetMinimapFloor();
        }
    }

    public RoomConnector[] GetDoors()
    {
        return doors;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            EnterRoom();
        }
    }

    public void SeeRoom()
    {
        minimapRoom.ShowRoom();
    }

    private void EnterRoom()
    {
        minimapRoom.VisitRoom();
        fog.SetActive(false);
        Room activeRoom = roomManager.GetActiveRoom();
        if (activeRoom != null)
        {
            List<Room> roomsToDisable = activeRoom.GetNeigbours();
            if (roomsToDisable.Contains(this)) roomsToDisable.Remove(this);
            foreach (Room room in roomsToDisable)
            {
                room.gameObject.SetActive(false);
            }
        }
        List<Room> roomsToActivate = GetNeigbours();
        if (roomsToActivate.Contains(activeRoom)) roomsToActivate.Remove(activeRoom);


        foreach (Room room in roomsToActivate)
        {
            room.SeeRoom();
            room.gameObject.SetActive(true);
        }

        roomManager.SetActiveRoom(this);
    }

    private void ExitRoom()
    {
        fog.SetActive(true);
        if(roomManager.GetActiveRoom() == this)
        {
            roomManager.SetActiveRoom(null);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(roomManager.GetActiveRoom() != this)
            {
                ExitRoom();
            }
            if (roomManager.GetActiveRoom() == null)
            {
                EnterRoom();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ExitRoom();
        }
    }

    public List<Room> GetNeigbours()
    {
        List<Room> neigbours = new List<Room>();
        foreach(RoomConnector door in doors)
        {
            RoomConnector neighbour = door.GetConnector();
            if (neighbour == null) continue;

            neigbours.Add(neighbour.GetRoom());
        }
        return neigbours;
    }

}
