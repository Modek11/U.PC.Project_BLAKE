using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConnector : MonoBehaviour
{
    [SerializeField]
    private RoomConnector connectedDoor;
   public void SetConnector(RoomConnector room)
    {
        connectedDoor = room;
    }

    public Transform GetTransform()
    {
        return gameObject.transform;
    }

    public RoomConnector GetConnector()
    {
        return connectedDoor;
    }
}
