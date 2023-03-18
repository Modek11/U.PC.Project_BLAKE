using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private RoomConnector[] doors;

    public RoomConnector[] GetDoors()
    {
        return doors;
    }
}
