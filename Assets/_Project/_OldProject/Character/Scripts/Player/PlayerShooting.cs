using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private PlayerInputSystem _controls;
    
    private GameObject _activeBulletPrefab;
    private Transform _gunHandlerObject;
    private GunSelector _gunSelector;

    private bool _isReloading;

    private byte _activeGunIndex;
    private byte _activeGunMagazineSize;
    private byte _activeGunBulletsLeft;
    private float _rateOfFire;
    private float _elapsedTimeForNextBullet;
    private float _reloadTime;
    private Transform _gunMuzzle;

    private UIPlayerController _uiController;

    void Awake()
    {
        //Game controls
        _controls = new PlayerInputSystem();
        _controls.Gameplay.Enable();

        //Guns variables
        _gunHandlerObject = GameObject.Find("GunHandler").transform;
        _isReloading = false;
        _elapsedTimeForNextBullet = 0;
        
        _gunSelector = GetComponent<GunSelector>();
        _activeGunIndex = 0;
        _activeGunBulletsLeft = 15;
        
        //UI
        _uiController = GetComponent<UIPlayerController>();
    }

    void Update()
    {
        if (_isReloading) return;
        
        Shoot();

        if (_controls.Gameplay.Reload.triggered)
        {
           StartCoroutine(Reload());
        }
        
    }

    private void LateUpdate()
    {
        _uiController.ReloadUI(_isReloading,_reloadTime);
    }

    public void ChangeGun(GunScriptableObject gun)
    {
        if(_isReloading) return;
        
        _gunSelector.SaveCurrentBulletsAmount(_activeGunIndex,_activeGunBulletsLeft);
        _gunHandlerObject.GetChild(_activeGunIndex).gameObject.SetActive(false);
        
        _activeGunIndex = gun.index;
        _activeGunMagazineSize = gun.magazineSize;
        _activeGunBulletsLeft = gun.bulletsLeft;
        _rateOfFire = gun.rateOfFire;
        _reloadTime = gun.reloadTime;
        _activeBulletPrefab = gun.bulletPrefab;

        
        _gunHandlerObject.GetChild(_activeGunIndex).gameObject.SetActive(true);
        _gunMuzzle = _gunHandlerObject.GetChild(_activeGunIndex).Find("muzzle").transform;
        
        _uiController.GunNameUI(gun.nameToDisplay);
        _uiController.MagazineSizeUI(_activeGunBulletsLeft,_activeGunMagazineSize);
    }
    
    private void Shoot()
    {
        if(_activeGunBulletsLeft <= 0) return;
        
        if(_elapsedTimeForNextBullet > 0)
        {
            _elapsedTimeForNextBullet -= Time.deltaTime;
            return;
        }

        if(_controls.Gameplay.Shooting.ReadValue<float>() > 0f)
        {
            Instantiate(_activeBulletPrefab, _gunMuzzle.position, _gunMuzzle.rotation);
            
            _elapsedTimeForNextBullet = _rateOfFire;
            _activeGunBulletsLeft -= 1;
            
            _uiController.MagazineSizeUI(_activeGunBulletsLeft,_activeGunMagazineSize);
        }
    }

    private IEnumerator Reload()
    {
        if (_activeGunBulletsLeft == _activeGunMagazineSize) yield break;
        
        
        _isReloading = true;
        yield return new WaitForSeconds(_reloadTime);
        _activeGunBulletsLeft = _activeGunMagazineSize;
        _isReloading = false;
        
        _uiController.MagazineSizeUI(_activeGunBulletsLeft,_activeGunMagazineSize);

    }

    private void OnDestroy()
    {
        _controls.Gameplay.Disable();
    }
}
