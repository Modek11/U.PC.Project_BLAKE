using TMPro;
using UnityEngine;

public class PlayerGameplayUIManager : MonoBehaviour
{
    [SerializeField]
    private FloorManager _floorManager;

    [SerializeField] 
    private MinimapCameraFollow minimapCamera;

    [SerializeField] 
    private RoomsDoneCounter roomsDoneCounter;

    [SerializeField, Space]
    private TextMeshProUGUI weaponName;

    [SerializeField]
    private TextMeshProUGUI bulletsLeft;

    [SerializeField]
    private TextMeshProUGUI roomsCounter;

    [SerializeField]
    private TextMeshProUGUI healthLeft;

    [SerializeField] 
    private GameObject interactUI;

    [SerializeField]
    private GameObject mapUI;

    [SerializeField] 
    private GameObject dashCooldownUI;
    
    private GameObject player;
    private WeaponsManager _weaponsManager;
    private PlayerInteractables playerInteractables;
    private BlakeCharacter blakeCharacter;
    private PlayerMovement _playerMovement;

    //private bool isMapShown = false;
    
    private void Start()
    {
        _floorManager.FloorGeneratorEnd += FloorManagerOnFloorGeneratorEnd;
        ReferenceManager.PlayerInputController.onMapPressEvent += ShowMap;
        ReferenceManager.PlayerInputController.onMapReleaseEvent += HideMap;
    }

    private void FloorManagerOnFloorGeneratorEnd(Transform playerTransform, Transform cameraFollowTransform)
    {
        if (player == null || _weaponsManager == null)
        {
            player = playerTransform.gameObject;
            _weaponsManager = player.GetComponent<WeaponsManager>();
            _weaponsManager.onSuccessfulShotEvent += RefreshUI;
            _weaponsManager.onPlayerPickupWeaponEvent += RefreshUI;
            _weaponsManager.changeWeaponEvent += RefreshUI;
            RefreshUI();
        }
        
        minimapCamera.SetPlayer(playerTransform);
        
        playerInteractables = player.GetComponent<PlayerInteractables>();
        _playerMovement = player.GetComponent<PlayerMovement>();
        blakeCharacter = player.GetComponent<BlakeCharacter>();

        playerInteractables.SetInteractUIReference(interactUI);
        _playerMovement.SetDashCooldownUIReference(dashCooldownUI);
    }
    
    private void ShowMap()
    {
        mapUI.SetActive(true);
    }

    private void HideMap()
    {
        mapUI.SetActive(false);
    }

    private void Update()
    {
        if (blakeCharacter is null) return;
        //Create events which updates those values only when they're changed 
        RoomsCounterUI();
        HealthLeftUI();
    }

    private void RefreshUI()
    {
        WeaponNameUI();
        BulletsLeftUI();
    }

    private void WeaponNameUI()
    {
        weaponName.text = _weaponsManager.GetWeaponDefinition(_weaponsManager.ActiveWeaponIndex).weaponName;
    }

    private void BulletsLeftUI()
    {
        if (weaponName.text == _weaponsManager.defaultWeapon.weaponName)
        {
            bulletsLeft.text = "∞";
        }
        else
        {
            Weapon weapon = _weaponsManager.GetIWeapon(_weaponsManager.ActiveWeaponIndex).GetWeapon().GetComponent<Weapon>();
            bulletsLeft.text = weapon.BulletsLeft.ToString();
        }
    }
    
    private void RoomsCounterUI()
    {
        roomsCounter.text = $"Rooms Beaten : {roomsDoneCounter.RoomsBeaten}/{roomsDoneCounter.RoomsInitialized}";
    }

    private void HealthLeftUI()
    {
        healthLeft.text = blakeCharacter.Health.ToString();
    }
    
}
