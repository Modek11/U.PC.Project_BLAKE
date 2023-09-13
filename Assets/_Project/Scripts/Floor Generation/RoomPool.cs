using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Room Pool", menuName = "Floor Generation/Room Pool")]
public class RoomPool : ScriptableObject
{
    [SerializeField]
    private GameObject[] room;

    public GameObject GetRandomRoomFromPool()
    {
        return room[Random.Range(0, room.Length)];
    }
}
