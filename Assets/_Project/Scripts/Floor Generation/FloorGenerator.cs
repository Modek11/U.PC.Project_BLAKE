using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FloorGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject startingRoom;

    [SerializeField]
    private List<GameObject> roomPool = new List<GameObject>();

    [SerializeField]
    private int maxRooms = 3;
    private int roomCounter = 1;

    private List<GameObject> spawnedRooms = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        GenerateFloor();
    }

    private void GenerateFloor()
    {
        GameObject _startingRoom = Instantiate(startingRoom, Vector3.zero, Quaternion.identity);
        spawnedRooms.Add(_startingRoom);

        while(roomCounter < maxRooms)
        {
            List<GameObject> toAdd = new List<GameObject>();
            foreach(GameObject room in spawnedRooms)
            {
                if (roomCounter >= maxRooms) break;
                Room roomScript = room.GetComponent<Room>();
                int randomNumber = Random.Range(0, roomScript.GetDoors().Length);
                RoomConnector door = roomScript.GetDoors()[randomNumber];

                if (door.GetConnector() != null) continue;

                GameObject newRoom = Instantiate(roomPool[Random.Range(0, roomPool.Count)]);
                int randomDoor = Random.Range(0, newRoom.GetComponent<Room>().GetDoors().Length);
                RoomConnector newDoor = newRoom.GetComponent<Room>().GetDoors()[randomDoor];

                Quaternion rot = Quaternion.LookRotation(-door.transform.forward);
                newRoom.transform.rotation = rot * Quaternion.Inverse(newDoor.transform.localRotation);

                Vector3 offset = newDoor.transform.position - door.transform.position;
                newRoom.transform.position -= offset;

                bool overlap = false;
                Collider[] colliders = Physics.OverlapBox(newRoom.transform.position, newRoom.GetComponent<BoxCollider>().size/2, newRoom.transform.rotation, LayerMask.NameToLayer("RoomOverlap"));

                foreach(Collider cols in colliders)
                {
                    if(cols.gameObject != newRoom && cols.gameObject.GetComponent<Room>() != null)
                    {
                        overlap = true;
                        Debug.Log("Overlapping");
                        break;
                    }
                }

                if(!overlap)
                {
                    door.SetConnector(newDoor);
                    newDoor.SetConnector(door);
                    toAdd.Add(newRoom);
                    roomCounter++;
                } else
                {
                    Destroy(newRoom);
                }

            }
            foreach(GameObject room in toAdd)
            {
                spawnedRooms.Add(room);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
