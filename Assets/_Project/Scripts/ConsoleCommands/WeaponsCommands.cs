using _Project.Scripts.GlobalHandlers;
using _Project.Scripts.Weapon;
using SickDev.CommandSystem;
using SickDev.DevConsole;
using UnityEngine;

namespace _Project.Scripts.ConsoleCommands
{
    public class WeaponsCommands : BaseCommand
    {
#if UNITY_EDITOR
        private readonly string NAME = "Weapons";
        
        [SerializeField]
        private WeaponPickup weaponPickup;
        [SerializeField] 
        private WeaponDefinitionHolder weaponDefinitionHolder;
        
        private bool infiniteAmmo;
        private bool infiniteAmmoSubscribed = false;
        
        protected override void Initialize()
        {
            base.Initialize();
            
            DevConsole.singleton.AddCommand(command = new ActionCommand<string>(SpawnGun) { className = NAME });
            commandsHolder.Add(command);
            
            DevConsole.singleton.AddCommand(command = new ActionCommand<string, string>(SpawnGun) { className = NAME });
            commandsHolder.Add(command);
            
            DevConsole.singleton.AddCommand(command = new ActionCommand<int>(AddAmmo) { className = NAME });
            commandsHolder.Add(command);
            
            DevConsole.singleton.AddCommand(command = new ActionCommand<bool>(InfiniteAmmo) { className = NAME });
            commandsHolder.Add(command);
        }

        private void SpawnGun(string weaponName)
        {
            weaponName = weaponName.ToLower();
            foreach (var weaponDefinition in weaponDefinitionHolder.ranged)
            {
                var weaponDefinitionName = weaponDefinition.WeaponName.ToLower();
                if (weaponName != weaponDefinitionName)
                {
                    continue;
                }
                
                var player = ReferenceManager.PlayerInputController.transform;
                var weaponPickupInstantiated = Instantiate(weaponPickup, player.position, Quaternion.identity);
                weaponPickupInstantiated.WeaponDefinition = weaponDefinition;
                return;
            }
        }
        
        private void SpawnGun(string weaponName,string weaponName2)
        {
            SpawnGun($"{weaponName} {weaponName2}");
        }
        
        private void AddAmmo(int amount)
        {
            var weaponsManager = ReferenceManager.PlayerInputController.GetComponent<WeaponsManager>();
            
            var weapon = weaponsManager.Weapons[weaponsManager.ActiveWeaponIndex];
            if (weapon != null && weapon is RangedWeapon rangedWeapon)
            {
                rangedWeapon.BulletsLeft += amount;
            }
        }

        private void InfiniteAmmo(bool obj)
        {
            infiniteAmmo = obj;
            var weaponsManager = ReferenceManager.PlayerInputController.GetComponent<WeaponsManager>();
            
            var secondWeapon = weaponsManager.Weapons[1];
            if (secondWeapon != null && secondWeapon is RangedWeapon currentRangedWeapon)
            {
                currentRangedWeapon.SetInfiniteAmmo(infiniteAmmo);
            }

            if (!infiniteAmmoSubscribed)
            {
                weaponsManager.OnPlayerPickupWeaponEvent += OnPlayerPickupWeapon;
                infiniteAmmoSubscribed = true;
            }
        }

        private void OnPlayerPickupWeapon(Weapon.Weapon weapon)
        {
            if (weapon != null && weapon is RangedWeapon rangedWeapon)
            {
                rangedWeapon.SetInfiniteAmmo(infiniteAmmo);
            }
        }
#endif
    }
}
