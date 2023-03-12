using AYellowpaper;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    public float Range;
    public float ReloadTime;
    public int MagazineSize;
    public int BulletsLeft = 10;
    public bool isLastShotOver;
    public Transform BulletsSpawnPoint;
    public GameObject BulletPrefab;
    
    [SerializeField] private bool allowButtonHold;
    [SerializeField] private InterfaceReference<IAttack> _primaryAttack;
    [SerializeField] private InterfaceReference<IAttack> _secondaryAttack;
    
    private bool isPlayerTryingShooting;
    private bool isPlayerTryingShooting2;
    private bool isReloading;
    
    private void Start()
    {
        BulletsLeft = MagazineSize;
        isLastShotOver = true;
    }

    private void Update()
    {
        TempInput();
    }

    private void TempInput()
    {
        isPlayerTryingShooting = allowButtonHold ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);
        isPlayerTryingShooting2 = allowButtonHold ? Input.GetKey(KeyCode.Mouse1) : Input.GetKeyDown(KeyCode.Mouse1);
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        
        PrimaryAttack();
        SecondaryAttack();
    }

    public void PrimaryAttack()
    {
        if (!CanShoot(isPlayerTryingShooting)) return;
        
        _primaryAttack.Value.Shot(this);
    }
    
    public void SecondaryAttack()
    {
        if (_secondaryAttack.Value is null) return;
        if (CanShoot(isPlayerTryingShooting2)) return;
        
        _secondaryAttack.Value.Shot(this);
    }
    
    public void ResetShot()
    {
        isLastShotOver = true;
    }
    
    public void Reload()
    {
        if (!(BulletsLeft < MagazineSize) || isReloading) return;
        
        isReloading = true;
        Invoke(nameof(FinishReload), ReloadTime);
    }
    
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    
    private bool CanShoot(bool isShotButtonPressed)
    {
        return isLastShotOver && isShotButtonPressed && !isReloading && BulletsLeft > 0;
    }
    
    private void FinishReload()
    {
        isReloading = false;
        BulletsLeft = MagazineSize;
    }
}
