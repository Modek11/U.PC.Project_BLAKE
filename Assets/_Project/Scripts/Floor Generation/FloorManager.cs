using System;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(FloorGenerator))]
public class FloorManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject virtualCameraPrefab;
    [SerializeField] private GameObject cameraFollowPrefab;

    private FloorGenerator floorGenerator;
    private GameObject player;
    private CinemachineVirtualCamera virtualCamera;
    private GameObject cameraFollow;
    
    
    public event Action<Transform,Transform> FloorGeneratorEnd;
    

    private void Awake()
    {
        floorGenerator = GetComponent<FloorGenerator>();
    }

    private void Start()
    {
        StartCoroutine(floorGenerator.GenerateFloor());
    }

    public void OnFloorGeneratorEnd(Transform startingRoomTransform)
    {
        player = Instantiate(playerPrefab, startingRoomTransform.position, Quaternion.identity);
        virtualCamera = Instantiate(virtualCameraPrefab).GetComponent<CinemachineVirtualCamera>();
        cameraFollow = Instantiate(cameraFollowPrefab);
        
        virtualCamera.Follow = cameraFollow.transform;
        cameraFollow.GetComponent<CameraFollowScript>().SetPlayerReference(player.transform);
        
        FloorGeneratorEnd?.Invoke(player.transform,cameraFollow.transform);
    }
}
