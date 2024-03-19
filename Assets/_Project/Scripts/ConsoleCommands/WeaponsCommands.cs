using _Project.Scripts.Weapon;
using SickDev.CommandSystem;
using SickDev.DevConsole;
using UnityEngine;

namespace _Project.Scripts.ConsoleCommands
{
    public class WeaponsCommands : BaseCommand
    {
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
            
            DevConsole.singleton.AddCommand(new ActionCommand<string>(SpawnGun) { className = NAME });
            DevConsole.singleton.AddCommand(new ActionCommand<string, string>(SpawnGun) { className = NAME });
            DevConsole.singleton.AddCommand(new ActionCommand<int>(AddAmmo) { className = NAME });
            DevConsole.singleton.AddCommand(new ActionCommand<bool>(InfiniteAmmo) { className = NAME });
        }

        private void SpawnGun(string weaponName)
        {
            foreach (var weaponDefinition in weaponDefinitionHolder.ranged)
            {
                if (weaponName != weaponDefinition.WeaponName)
                {
                    continue;
                }
                
                var player = ReferenceManager.PlayerInputController.transform;
                var weaponPickupInstantiated = Instantiate(weaponPickup, player.position, Quaternion.identity);
                weaponPickupInstantiated.WeaponDefinition = weaponDefinition;
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
            var rangedWeapon = (RangedWeapon)weapon;
            rangedWeapon.BulletsLeft = rangedWeapon.BulletsLeft + amount;
        }

        private void InfiniteAmmo(bool obj)
        {
            infiniteAmmo = obj;
            var weaponsManager = ReferenceManager.PlayerInputController.GetComponent<WeaponsManager>();

            if (!infiniteAmmoSubscribed)
            {
                weaponsManager.OnPlayerPickupWeaponEvent += OnPlayerPickupWeapon;
                infiniteAmmoSubscribed = true;
            }
        }

        private void OnPlayerPickupWeapon(global::Weapon weapon)
        {
            var rangedWeapon = (RangedWeapon)weapon;
            rangedWeapon.SetInfiniteAmmo(infiniteAmmo);
        }
    }
}
