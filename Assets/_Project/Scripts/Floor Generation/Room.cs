using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private RoomConnector[] doors;
    [SerializeField]
    private RandomizedRoomObject[] randomObjects;

    public void InitializeRoom()
    {
        foreach(RandomizedRoomObject randomObject in randomObjects)
        {
            randomObject.InitializeRandomObject();
        }
    }

    public RoomConnector[] GetDoors()
    {
        return doors;
    }
}
