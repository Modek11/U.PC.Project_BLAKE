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

    public void InitializeRoom()
    {
        foreach(RandomizedRoomObject randomObject in randomObjects)
        {
            randomObject.InitializeRandomObject();
        }
        foreach(RoomConnector door in doors)
        {
            door.SetDoor();
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
            fog.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fog.SetActive(true);
        }
    }
}
