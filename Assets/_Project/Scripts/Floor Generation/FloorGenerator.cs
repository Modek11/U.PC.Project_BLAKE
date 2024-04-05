using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Header("Next Level Room")]
    [SerializeField]
    private GameObject endRoom;

    [SerializeField]
    private GameObject startingRoom;

    [SerializeField]
    private LayerMask layerMask;

    private int roomCounter = 1;

    private RoomManager roomManager;
    private FloorManager floorManager;

    private List<GameObject> spawnedRooms = new List<GameObject>();
    private List<GameObject> spawnedBaseRooms = new List<GameObject>();
    private GameObject map;
    private int maxRooms = 0;
    private int tries = 0;
    private int endRoomTries = 0;

    private Dictionary<Room, int> pathLength = new Dictionary<Room, int>();

    [Header("Debugs")]
    [SerializeField]
    private bool debugOverlapping = false;
    [SerializeField]
    private bool debugRoomFinding = false;
    public int MaxRooms => maxRooms;

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
        yield return StartCoroutine(GenerateBaseRoomsFromPool(baseRoomPool, maxBaseRooms));

        //Generate End Room
        tries = 0;
        GenerateEndRoom();

        tries = 0;
        //Generate TreasureRooms
        yield return StartCoroutine(GenerateSpecialRoomsFromPool(treasureRoomPool, treasureRoomsAmount));


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

    private IEnumerator GenerateBaseRoomsFromPool(RoomPool pool, int amountOfRooms)
    {
        int tempCounter = 0;
        FloorPathfinding floorPathfinding = new FloorPathfinding(spawnedRooms);

        while (tempCounter < amountOfRooms) {
            if (tries >= 200)
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

                bool success = TryPositionRoom(room, newRoom, door, newDoor);

                if (success)
                {
                    FinalizeRoomPlacement(newRoom, door, newDoor, map);
                    toAdd.Add(newRoom);
                    tempCounter++;
                }
                else
                {
                    Destroy(newRoom);
                }

                yield return new WaitForFixedUpdate();
            }
            foreach (GameObject room in toAdd)
            {
                spawnedRooms.Add(room);
                spawnedBaseRooms.Add(room);
                if (pathLength.ContainsKey(room.GetComponent<Room>())) continue;
                floorPathfinding = new FloorPathfinding(spawnedRooms);
                List<Room> path = floorPathfinding.FindPath(spawnedRooms[0], room);
                pathLength.Add(room.GetComponent<Room>(), path.Count);
            }

        }
        
    }
    private IEnumerator GenerateSpecialRoomsFromPool(RoomPool pool, int amountOfRooms)
    {
        int tempCounter = 0;
        FloorPathfinding floorPathfinding = new FloorPathfinding(spawnedRooms);
        var list = pathLength.OrderBy(key => key.Value).Reverse().Select(key => key.Key.gameObject).ToList();
        if(debugRoomFinding) Debug.Log("Furthest room is: " + list[0].gameObject.name);
        while (tempCounter < amountOfRooms)
        {
            if (tries >= 50)
            {
                Debug.LogWarning("Broken generation due to too many tries");
                break;
            }


            List<GameObject> toAdd = new List<GameObject>();

            foreach (GameObject room in list)
            {
                if (tempCounter >= amountOfRooms) break;
                Room roomScript = room.GetComponent<Room>();

                if(roomScript.GetRoomType() != RoomType.Base) { continue; }

                RoomConnector door = null;
                RoomConnector[] doors = roomScript.GetDoors();
                var randomizedDoors = doors.OrderBy(c => Random.Range(0, doors.Length));

                foreach (RoomConnector d in randomizedDoors) {
                    if (d.GetConnector() != null)
                    {
                        continue;
                    } else
                    {
                        door = d;
                        break;
                    }
                }
                if (door == null) continue;
                GameObject newRoom = Instantiate(pool.GetRandomRoomFromPool());
                int randomDoor = Random.Range(0, newRoom.GetComponent<Room>().GetDoors().Length);
                RoomConnector newDoor = newRoom.GetComponent<Room>().GetDoors()[randomDoor];

                bool success = TryPositionRoom(room, newRoom, door, newDoor);

                if (success)
                {
                    FinalizeRoomPlacement(newRoom, door, newDoor, map);
                    toAdd.Add(newRoom);
                    tempCounter++;
                }
                else
                {
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

    }

    public void GenerateEndRoom()
    {
        int tempCounter = 0;
        FloorPathfinding floorPathfinding = new FloorPathfinding(spawnedRooms);

        if (tries >= 200)
        {
            Debug.LogWarning("Broken generation due to too many tries");
            return;
        }


        List<GameObject> toAdd = new List<GameObject>();

        foreach (GameObject room in spawnedRooms)
        {
            Room roomScript = room.GetComponent<Room>();
            int randomNumber = Random.Range(0, roomScript.GetDoors().Length);
            RoomConnector door = roomScript.GetDoors()[randomNumber];
            if (door.GetConnector() != null) continue;
            GameObject newRoom = Instantiate(endRoom);
            int randomDoor = Random.Range(0, newRoom.GetComponent<Room>().GetDoors().Length);
            RoomConnector newDoor = newRoom.GetComponent<Room>().GetDoors()[randomDoor];

            bool success = TryPositionRoom(room, newRoom, door, newDoor);

            if (success)
            {
                FinalizeRoomPlacement(newRoom, door, newDoor, map);
                toAdd.Add(newRoom);
                tempCounter++;
                break;
            }
            else
            {
                Destroy(newRoom);
            }

        }

        if(toAdd.Count == 0)
        {
            if(endRoomTries < 100)
            {
                endRoomTries++;
                GenerateEndRoom();
                return;
            }
            Debug.LogError("END ROOM DIDN'T SPAWNED");
        }

        foreach (GameObject room in toAdd)
        {
            spawnedRooms.Add(room);
            spawnedBaseRooms.Add(room);
            if (pathLength.ContainsKey(room.GetComponent<Room>())) continue;
            floorPathfinding = new FloorPathfinding(spawnedRooms);
            List<Room> path = floorPathfinding.FindPath(spawnedRooms[0], room);
            pathLength.Add(room.GetComponent<Room>(), path.Count);
        }
    }

    private bool TryPositionRoom(GameObject room, GameObject newRoom, RoomConnector doorToConnect, RoomConnector newDoor)
    {

        // Calculate the rotation needed to match the new door with the door to connect
        Quaternion rotationToMatchDoors = Quaternion.LookRotation(-doorToConnect.transform.forward);
        newRoom.transform.rotation = rotationToMatchDoors * Quaternion.Inverse(newDoor.transform.rotation);

        // Calculate the position offset to match the new door with the door to connect
        Vector3 positionOffset = newDoor.transform.position - doorToConnect.transform.position;
        newRoom.transform.position -= positionOffset;

        // Check for any overlaps with existing rooms
        bool isOverlapping = CheckForOverlap(room, newRoom);
        if (isOverlapping)
        {
            tries++; // Increment tries if there is an overlap
            if (debugOverlapping) Debug.Log($"Overlap detected on attempt {tries} for room: {newRoom.name}");
        }
        return !isOverlapping;
    }

    private bool CheckForOverlap(GameObject room, GameObject newRoom)
    {

        BoxCollider[] roomColliders = newRoom.GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider collider in roomColliders)
        {
            Collider[] overlaps = Physics.OverlapBox(
                newRoom.transform.TransformPoint(collider.center + collider.transform.localPosition),
                collider.size / 2,
                newRoom.transform.rotation,
                layerMask,
                QueryTriggerInteraction.Collide
            );

            foreach (Collider overlap in overlaps)
            {
                if (overlap.gameObject != newRoom && overlap.gameObject.GetComponent<RoomOverlapTrigger>() != null && overlap.gameObject != room)
                {
                    // If any overlaps are detected with objects other than the room itself, return true.
                    if (debugOverlapping) Debug.Log("Overlapping detected with: " + overlap.gameObject.name);
                    return true;
                }
            }
        }
        // If no overlaps are detected, return false.
        return false;
    }

    private void FinalizeRoomPlacement(GameObject newRoom, RoomConnector doorToConnect, RoomConnector newDoor, GameObject map)
    {

        doorToConnect.SetConnector(newDoor);
        newDoor.SetConnector(doorToConnect);
        newRoom.GetComponent<Room>().SetupDoorConnectors();
        doorToConnect.GetRoom().GetComponent<Room>().SetupDoorConnectors();
        newRoom.transform.parent = map.transform;
        roomCounter++;

        if (ReferenceManager.SceneHandler != null)
        {
            ReferenceManager.SceneHandler.roomsGenerated++;
        }
    }
}
