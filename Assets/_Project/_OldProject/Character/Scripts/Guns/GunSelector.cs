using UnityEngine;

public class GunSelector : MonoBehaviour
{
    [SerializeField] private GunScriptableObject[] gunScriptableObjects;
    private PlayerInputSystem _controls;
    private PlayerShooting _playerShooting;
    private int _chosenGunIndex;


    private void Awake()
    {
        //Nie zmienia³em tu input systemu bo i tak jak dla mnie trzeba to wszystko zmieniæ
        _controls = new PlayerInputSystem();
        _controls.Gameplay.Enable();
        _playerShooting = GetComponent<PlayerShooting>();
        ResetBullets();
        _playerShooting.ChangeGun(gunScriptableObjects[0]);
    }

    private void Update()
    {
        _controls.Gameplay.ChooseGun.performed += ctx =>
        {
            _chosenGunIndex = (int)ctx.ReadValue<float>() - 1;
            _playerShooting.ChangeGun(gunScriptableObjects[_chosenGunIndex]);
        };
    }

    private void ResetBullets()
    {
        foreach (var gun in gunScriptableObjects)
        {
            gun.bulletsLeft = gun.magazineSize;
        }
    }

    public void SaveCurrentBulletsAmount(byte index, byte bullets)
    {
        gunScriptableObjects[index].bulletsLeft = bullets;
    }

    private void OnDestroy()
    {
        _controls.Gameplay.Disable();
    }
}
