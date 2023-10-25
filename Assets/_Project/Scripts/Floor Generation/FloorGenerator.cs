using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(FloorManager))]
public class FloorGenerator : MonoBehaviour
{

    [SerializeField]
    private int seed = 0;

    [Header("Base Rooms Settings")]
    [SerializeField]
    private RoomPool baseRoomPool;
    [SerializeField]
    private int maxBaseRooms = 3;

    [Header("Treasure Room Settings")]
    [SerializeField]
    private RoomPool treasureRoomPool;
    [SerializeField]
    private int treasureRoomsAmount = 1;

    [SerializeField]
    private GameObject startingRoom;

    [SerializeField]
    private LayerMask layerMask;

    private int roomCounter = 1;

    private RoomManager roomManager;
    private FloorManager floorManager;

    private List<GameObject> spawnedRooms = new List<GameObject>();
    private GameObject map;
    private int maxRooms = 0;
    private int tries = 0;

    private Dictionary<Room, int> pathLength = new Dictionary<Room, int>();
    public int GetIntRoomsInitialized()
    {
        return maxRooms;
    }
    
    public IEnumerator GenerateFloor()
    {
        maxRooms = maxBaseRooms + treasureRoomsAmount;
        if (ReferenceManager.SceneHandler != null)
        {
            ReferenceManager.SceneHandler.roomsToGenerate = maxRooms - 1;
            while (!ReferenceManager.SceneHandler.isSceneLoadedProperly) yield return null;
        }
        
        floorManager = GetComponent<FloorManager>();
        roomManager = GetComponent<RoomManager>();

        //Creating empty objects for easier search
        map = new GameObject("Map");
        GameObject triggersObject = new GameObject("Triggers");

        seed = (seed == 0) ? Random.Range(int.MinValue, int.MaxValue) : seed;
        Random.InitState(seed);
        Debug.Log("Using seed: " + seed);
        GameObject _startingRoom = Instantiate(startingRoom, Vector3.zero, Quaternion.identity);
        spawnedRooms.Add(_startingRoom);
        startingRoom.GetComponent<Room>().SetupDoorConnectors();

        tries = 0;
        //Generate Base Floors
        yield return StartCoroutine(GenerateRoomsFromPool(baseRoomPool, maxBaseRooms));

        tries = 0;
        //Generate TreasureRooms
        yield return StartCoroutine(GenerateRoomsFromPool(treasureRoomPool, treasureRoomsAmount, true));

        foreach (GameObject room in spawnedRooms)
        {
            Room roomScript = room.GetComponent<Room>();
            roomScript.InitializeRoom(roomManager);
            RoomTrigger[] triggers = room.GetComponentsInChildren<RoomTrigger>();
            foreach(RoomTrigger trigger in triggers)
            {
                trigger.transform.parent = triggersObject.transform;
            }
            if(room != _startingRoom) room.SetActive(false);
        }
        foreach(Room room in startingRoom.GetComponent<Room>().GetNeigbours())
        {
            room.gameObject.SetActive(true);
        }
        roomManager.SetActiveRoom(_startingRoom.GetComponent<Room>());
        _startingRoom.GetComponent<Room>().SeeRoom();

        floorManager.OnFloorGeneratorEnd(_startingRoom.GetComponent<Room>().GetSpawnPointPosition());

        
    }

    public GameObject GetMapObject()
    {
        return map;
    }

    private IEnumerator GenerateRoomsFromPool(RoomPool pool, int amountOfRooms, bool usePathfinding = false)
    {
        int tempCounter = 0;
        FloorPathfinding floorPathfinding = new FloorPathfinding(spawnedRooms);

        while (tempCounter < amountOfRooms) {
            if (tries >= 50)
            {
                Debug.LogWarning("Broken generation due to too many tries");
                break;
            }

            
            List<GameObject> toAdd = new List<GameObject>();
            foreach (GameObject room in spawnedRooms)
            {
                if (tempCounter >= amountOfRooms) break;
                Room roomScript = room.GetComponent<Room>();
                int randomNumber = Random.Range(0, roomScript.GetDoors().Length);
                RoomConnector door = roomScript.GetDoors()[randomNumber];

                if (door.GetConnector() != null) continue;

                GameObject newRoom = Instantiate(pool.GetRandomRoomFromPool());
                int randomDoor = Random.Range(0, newRoom.GetComponent<Room>().GetDoors().Length);
                RoomConnector newDoor = newRoom.GetComponent<Room>().GetDoors()[randomDoor];

                Quaternion rot = Quaternion.LookRotation(-door.transform.forward);
                newRoom.transform.rotation = rot * Quaternion.Inverse(newDoor.transform.rotation);

                Vector3 offset = newDoor.transform.position - door.transform.position;
                newRoom.transform.position -= offset;
                bool overlap = false;
                BoxCollider[] roomColliders = newRoom.GetComponent<Room>().GetOverlapColliders();
                foreach (BoxCollider box in roomColliders)
                {
                    if (overlap) break;
                    Collider[] colliders = Physics.OverlapBox(newRoom.transform.TransformPoint(box.center), box.size / 2, newRoom.transform.rotation, layerMask, QueryTriggerInteraction.Collide);

                    foreach (Collider cols in colliders)
                    {
                        if (cols.gameObject != newRoom && cols.gameObject.GetComponent<RoomOverlapTrigger>() != null && cols.gameObject != room)
                        {
                            overlap = true;
                            tries++;
                            Debug.Log("Overlapping at " + room.name);
                            break;
                        }
                    }
                }
                if (!overlap)
                {
                    tries = 0;
                    door.SetConnector(newDoor);
                    newDoor.SetConnector(door);
                    newRoom.GetComponent<Room>().SetupDoorConnectors();
                    toAdd.Add(newRoom);
                    newRoom.transform.parent = map.transform;
                    roomCounter++;
                    tempCounter++;

                    if (ReferenceManager.SceneHandler != null)
                        ReferenceManager.SceneHandler.roomsGenerated++;
                }
                else
                {
                    Debug.Log("Destroying" + newRoom.name);
                    Destroy(newRoom);
                }

                yield return new WaitForFixedUpdate();
            }
            foreach (GameObject room in toAdd)
            {
                spawnedRooms.Add(room);
                if (pathLength.ContainsKey(room.GetComponent<Room>())) continue;
                floorPathfinding = new FloorPathfinding(spawnedRooms);
                List<Room> path = floorPathfinding.FindPath(spawnedRooms[0], room);
                pathLength.Add(room.GetComponent<Room>(), path.Count);
            }

        }
        if (usePathfinding)
        {
            var newPaths = pathLength.OrderBy(key => key.Value).Reverse();

            foreach (KeyValuePair<Room, int> kvp in newPaths)
            {
                Debug.Log("Room: " + kvp.Key.gameObject.name + "\nPath Length: " + kvp.Value);
            }
        }
    }
}
