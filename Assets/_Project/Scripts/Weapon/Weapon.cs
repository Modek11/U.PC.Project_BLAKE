using AYellowpaper;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour, IWeapon
{
    //TODO: Implement range
    public float Range;
    public float ReloadTime;
    public int MagazineSize;
    public Transform BulletsSpawnPoint;
    public GameObject BulletPrefab;
    [HideInInspector] public bool isLastShotOver;
    [HideInInspector] public int BulletsLeft = 10;
    //TODO: Move it outside
    [HideInInspector] public AudioSource As;
    [Header("Attacks")]
    [Tooltip("Attack which will be triggered on LMB")]
    [SerializeField] private InterfaceReference<IAttack> _primaryAttack;
    [Tooltip("Attack which will be triggered on RMB, it's not required")]
    [SerializeField] private InterfaceReference<IAttack> _secondaryAttack;

    private bool isReloading;

    private void Awake()
    {
        As = GetComponent<AudioSource>();
    }

    private void Start()
    {
        BulletsLeft = MagazineSize;
        isLastShotOver = true;
    }

    public void PrimaryAttack()
    {
        if (!CanShoot()) return;
        _primaryAttack.Value.Attack(this);
    }

    public float GetCurrentWeaponFireRate()
    {
        return _primaryAttack.Value.ReturnFireRate();
    }
    
    public void SecondaryAttack()
    {
        if (_secondaryAttack.Value is null) return;
        if (!CanShoot()) return;
        _secondaryAttack.Value.Attack(this);
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
    
    private bool CanShoot()
    {
        return isLastShotOver && !isReloading && BulletsLeft > 0;
    }
    
    private void FinishReload()
    {
        isReloading = false;
        BulletsLeft = MagazineSize;
    }

    private void OnValidate()
    {
        Assert.IsNotNull(_primaryAttack.Value);
    }
}
