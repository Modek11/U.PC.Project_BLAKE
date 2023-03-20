using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FloorGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject startingRoom;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private List<GameObject> roomPool = new List<GameObject>();

    [SerializeField]
    private int maxRooms = 3;
    private int roomCounter = 1;

    private List<GameObject> spawnedRooms = new List<GameObject>();
    // Start is called before the first frame update
    IEnumerator Start()
    {
        GameObject _startingRoom = Instantiate(startingRoom, Vector3.zero, Quaternion.identity);
        spawnedRooms.Add(_startingRoom);
        int tries = 0;
        while (roomCounter < maxRooms)
        {
            if (tries >= 10)
            {
                Debug.LogWarning("Broken generation due to too many tries");
                break;
            }
            List<GameObject> toAdd = new List<GameObject>();
            foreach (GameObject room in spawnedRooms)
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
                BoxCollider[] roomColliders = newRoom.GetComponents<BoxCollider>();
                foreach (BoxCollider box in roomColliders)
                {
                    if (overlap) break;
                    Collider[] colliders = Physics.OverlapBox(newRoom.transform.position + box.center, box.size / 2 + new Vector3(-0.05f, -0.05f, -0.05f), newRoom.transform.rotation, layerMask, QueryTriggerInteraction.Collide);

                    foreach (Collider cols in colliders)
                    {
                        if (cols.gameObject != newRoom && cols.gameObject.GetComponent<Room>() != null && cols.gameObject != room)
                        {
                            overlap = true;
                            tries++;
                            Debug.Log("Overlapping");
                            break;
                        }
                    }
                }
                if (!overlap)
                {
                    tries = 0;
                    door.SetConnector(newDoor);
                    newDoor.SetConnector(door);
                    toAdd.Add(newRoom);
                    roomCounter++;
                }
                else
                {
                    Debug.Log("Destroying" + newRoom.name);
                    newRoom.SetActive(false);
                }

                yield return new WaitForFixedUpdate();
            }
            foreach (GameObject room in toAdd)
            {
                spawnedRooms.Add(room);
            }
        }
    }

    private void GenerateFloor()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
