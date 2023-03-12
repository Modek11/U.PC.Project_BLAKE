using AYellowpaper;
using UnityEngine;

public class Glock : MonoBehaviour, IWeapon
{
    public float TimeBetweenShooting;
    public float Spread;
    public float Range;
    public float ReloadTime;
    public float TimeBetweenShots;
    public int MagazineSize;
    public int bulletsLeft = 10;

    private bool shooting;
    public bool readyToShoot;
    [SerializeField] private bool allowButtonHold;
    private bool reloading;
    [SerializeField] public Transform attackPoint;
    [SerializeField] public GameObject bulletPrefab;

    [SerializeField] private InterfaceReference<IAttack> _primaryAttack;
    [SerializeField] private InterfaceReference<IAttack> _secondaryAttack;

    private void Start()
    {
        bulletsLeft = MagazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        TempInput();
    }

    private void TempInput()
    {
        shooting = allowButtonHold ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        
        PrimaryAttack();
        
    }

    public void PrimaryAttack()
    {
        if (!readyToShoot || !shooting || reloading || bulletsLeft <= 0) return;
        
        _primaryAttack.Value.Shot(this);
    }
    
    public void SecondaryAttack()
    {
        if (_secondaryAttack is null) return;
        if (!readyToShoot || !shooting || reloading || bulletsLeft <= 0) return;
        _secondaryAttack.Value.Shot(this);
    }

    public void ResetShot()
    {
        readyToShoot = true;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    private void Reload()
    {
        if (!(bulletsLeft < MagazineSize) || reloading) return;
        
        reloading = true;
        Invoke(nameof(FinishReload), ReloadTime);
    }

    private void FinishReload()
    {
        reloading = false;
        bulletsLeft = MagazineSize;
    }
}
