using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(FloorGenerator))]
public class FloorManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private CinemachineVirtualCamera cvPlayerCam;
    [SerializeField]
    private MinimapCameraFollow minimapCamera;
    [SerializeField]
    private PlayerUIObject playerUI;
    private FloorGenerator floorGenerator;

    private void Awake()
    {
        floorGenerator = GetComponent<FloorGenerator>();
    }

    private void Start()
    {
        StartCoroutine(floorGenerator.GenerateFloor());
    }

    public void OnGenerationEnd(Vector3 playerSpawnPosition)
    {
        GameObject player = Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);
        player.GetComponent<PlayerMovement>().SetMainCamera(mainCamera);
        minimapCamera.SetPlayer(player.transform);
        player.GetComponent<UIPlayerController>().SetUI(playerUI);
        cvPlayerCam.Follow = player.transform;
        cvPlayerCam.LookAt = player.transform;
    }
}
