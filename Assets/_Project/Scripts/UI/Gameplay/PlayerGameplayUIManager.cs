using System.Collections;
using _Project.Scripts.Floor_Generation;
using _Project.Scripts.PointsSystem;
using _Project.Scripts.Weapon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Gameplay
{
    public class PlayerGameplayUIManager : MonoBehaviour
    {
        private const string INFINITY_SYMBOL = "∞";
    
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
        private TextMeshProUGUI pointsCounter;
    
        [SerializeField] 
        private TextMeshProUGUI killsCounter;
    
        [SerializeField] 
        private TextMeshProUGUI comboCounter;

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
        }

        private void FloorManagerOnFloorGeneratorEnd(Transform playerTransform, Transform cameraFollowTransform)
        {
            ReferenceManager.PlayerInputController.onMapPressEvent += ShowMap;
            ReferenceManager.PlayerInputController.onMapReleaseEvent += HideMap;
            floorManager.FloorGeneratorEnd -= FloorManagerOnFloorGeneratorEnd;
        
            if (player == null || weaponsManager == null)
            {
                player = playerTransform.gameObject;
                weaponsManager = player.GetComponent<WeaponsManager>();
                weaponsManager.OnPrimaryAttack += RefreshUI;
                weaponsManager.OnPlayerPickupWeaponEvent += RefreshUI;
                weaponsManager.OnWeaponChangedEvent += RefreshUI;
                RefreshUI(weaponsManager.Weapons[weaponsManager.ActiveWeaponIndex]);
            }
        
            minimapCamera.SetPlayer(playerTransform);
        
            playerInteractables = player.GetComponent<PlayerInteractables>();
            blakeCharacter = player.GetComponent<BlakeCharacter>();
            playerMovement = player.GetComponent<PlayerMovement>();

            playerInteractables.SetInteractUIReference(interactUI);
            dashCooldownImage = dashCooldownUI.transform.GetChild(1).GetComponent<Image>();

            EnemyDeathMediator.Instance.OnRegisteredEnemyDeath += UpdatePointsAndCombo;
            EnemyDeathMediator.Instance.ComboController.OnComboTimerEnd += HideComboTexts;
            HideComboTexts();
        
            playerMovement.OnDashPerformed += StartDashCooldownUI;
            roomsDoneCounter.OnRoomBeaten += RoomsCounterUI;
            blakeCharacter.OnDamageTaken += HealthLeftUI;
            blakeCharacter.onRespawn += OnRespawnUIUpdate;
            OnRespawnUIUpdate();
            RoomsCounterUI();

        }

        private void ShowMap()
        {
            mapUI.SetActive(true);
        }

        private void HideMap()
        {
            mapUI.SetActive(false);
        }

        private void RefreshUI(Weapon.Weapon weapon)
        {
            WeaponNameUI(weapon);
            BulletsLeftUI(weapon);
        }

        private void WeaponNameUI(Weapon.Weapon weapon)
        {
            weaponName.text = weapon.WeaponDefinition.WeaponName;
        }

        private void BulletsLeftUI(Weapon.Weapon weapon)
        {
            RangedWeapon rangedWeapon = weapon as RangedWeapon;

            bulletsLeft.text = rangedWeapon != null ? rangedWeapon.BulletsLeft.ToString() : INFINITY_SYMBOL;
        }
        
          private void RoomsCounterUI()
        {
            roomsCounter.text = $"Rooms Beaten : {roomsDoneCounter.RoomsBeaten}/{roomsDoneCounter.RoomsInitialized}";
        }

        private void HealthLeftUI(GameObject instigator)
        {
            healthLeft.text = blakeCharacter.Health.ToString();
        }

        private void OnRespawnUIUpdate()
        {
            healthLeft.text = blakeCharacter.Health.ToString();
        }

        private void UpdatePointsAndCombo(ComboAndPointsValues comboAndPointsValues)
        {
            pointsCounter.text = $"Points: {comboAndPointsValues.Points}";
            
            if (!comboAndPointsValues.ShouldComboStart)
            {
                return;
            }
        
            var killsCounterActive = killsCounter.gameObject.activeInHierarchy;
            var comboCounterActive = comboCounter.gameObject.activeInHierarchy;
        
            if (!killsCounterActive || !comboCounterActive)
            {
                killsCounter.gameObject.SetActive(true);
                comboCounter.gameObject.SetActive(true);
            }
        
            killsCounter.text = $"x{comboAndPointsValues.KillsCounter} KILLS";
            comboCounter.text = $"x{comboAndPointsValues.ComboCounter} Points";
        }

        private void HideComboTexts()
        {
            var killsCounterActive = killsCounter.gameObject.activeInHierarchy;
            var comboCounterActive = comboCounter.gameObject.activeInHierarchy;
            if (killsCounterActive || comboCounterActive)
            {
                killsCounter.gameObject.SetActive(false);
                comboCounter.gameObject.SetActive(false);
            }
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
                dashCooldownUI.transform.position = player.transform.position + Vector3.up * 0.2f;
                dashCooldownUI.transform.LookAt(Camera.main.transform);

                yield return new WaitForEndOfFrame();
            }
        
            dashCooldownUI.SetActive(false);
            dashCooldownImage.fillAmount = 0;
        }

        private void OnDestroy()
        {
            playerMovement.OnDashPerformed -= StartDashCooldownUI;
            roomsDoneCounter.OnRoomBeaten -= RoomsCounterUI;
            blakeCharacter.OnDamageTaken -= HealthLeftUI;
            blakeCharacter.onRespawn -= OnRespawnUIUpdate;
        }
    }
}
