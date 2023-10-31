using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGameplayUIManager : MonoBehaviour
{
    [SerializeField] 
    private FloorManager floorManager;

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
    private WeaponsManager weaponsManager;
    private PlayerInteractables playerInteractables;
    private BlakeCharacter blakeCharacter;
    private PlayerMovement playerMovement;
    private Image dashCooldownImage;

    private void Start()
    {
        floorManager.FloorGeneratorEnd += FloorManagerOnFloorGeneratorEnd;
        ReferenceManager.PlayerInputController.onMapPressEvent += ShowMap;
        ReferenceManager.PlayerInputController.onMapReleaseEvent += HideMap;
    }

    private void FloorManagerOnFloorGeneratorEnd(Transform playerTransform, Transform cameraFollowTransform)
    {
        if (player == null || weaponsManager == null)
        {
            player = playerTransform.gameObject;
            weaponsManager = player.GetComponent<WeaponsManager>();
            weaponsManager.onSuccessfulShotEvent += RefreshUI;
            weaponsManager.onPlayerPickupWeaponEvent += RefreshUI;
            weaponsManager.changeWeaponEvent += RefreshUI;
            RefreshUI();
        }
        
        minimapCamera.SetPlayer(playerTransform);
        
        playerInteractables = player.GetComponent<PlayerInteractables>();
        blakeCharacter = player.GetComponent<BlakeCharacter>();
        playerMovement = player.GetComponent<PlayerMovement>();

        playerInteractables.SetInteractUIReference(interactUI);
        dashCooldownImage = dashCooldownUI.transform.GetChild(1).GetComponent<Image>();
        
        playerMovement.OnDashPerformed += StartDashCooldownUI;
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
        weaponName.text = weaponsManager.GetWeaponDefinition(weaponsManager.ActiveWeaponIndex).weaponName;
    }

    private void BulletsLeftUI()
    {
        if (weaponName.text == weaponsManager.defaultWeapon.weaponName)
        {
            bulletsLeft.text = "∞";
        }
        else
        {
            Weapon weapon = weaponsManager.GetIWeapon(weaponsManager.ActiveWeaponIndex).GetWeapon().GetComponent<Weapon>();
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

    private void StartDashCooldownUI()
    {
        StartCoroutine(DashCooldownUI());
    }

    private IEnumerator DashCooldownUI()
    {
        dashCooldownUI.SetActive(true);
        
        while(playerMovement.DashCooldownCountdown > 0)
        {
            dashCooldownImage.fillAmount = playerMovement.DashCooldownCountdown/playerMovement.DashCooldown;
            dashCooldownUI.transform.position = player.transform.position + Vector3.up * 0.6f;
            dashCooldownUI.transform.LookAt(Camera.main.transform);

            yield return new WaitForEndOfFrame();
        }
        
        dashCooldownUI.SetActive(false);
        dashCooldownImage.fillAmount = 0;
        
    }
    
}
