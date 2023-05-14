using TMPro;
using UnityEngine;

[RequireComponent(typeof(PlayerInputController))]
public class PlayerGameplayUIManager : MonoBehaviour
{
    [SerializeField] private FloorManager _floorManager;
    [SerializeField] private MinimapCameraFollow minimapCamera;
    [SerializeField] private RoomsDoneCounter roomsDoneCounter;
    [Space]
    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private TextMeshProUGUI bulletsLeft;
    [SerializeField] private TextMeshProUGUI roomsCounter;
    [SerializeField] private TextMeshProUGUI healthLeft;
    [SerializeField] private GameObject interactUI;

    private GameObject player;
    private WeaponsManager _weaponsManager;
    private PlayerInteractables playerInteractables;
    private BlakeCharacter blakeCharacter;
    
    private void Start()
    {
        _floorManager.FloorGeneratorEnd += FloorManagerOnFloorGeneratorEnd;
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
        blakeCharacter = player.GetComponent<BlakeCharacter>();
        playerInteractables.SetInteractUIReference(interactUI);
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
            Weapon weapon = _weaponsManager.GetIWeapon(_weaponsManager.ActiveWeaponIndex).GetGameObject().GetComponent<Weapon>();
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
