using AYellowpaper;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour, IWeapon
{
    private GameObject owner;

    //TODO: Implement range
    public float Range;
    private int MagazineSize;
    public bool infinityAmmo = false;
    public Transform BulletsSpawnPoint;
    public GameObject BulletPrefab;
    private bool outOfAmmo = false;
    private int bulletsLeft = 0;
    public int BulletsLeft
    {
        get => bulletsLeft;
        set
        {
            if (bulletsLeft != value)
            {
                bulletsLeft = value;

                if(bulletsLeft <= 0)
                {
                    outOfAmmo = true;
                }
            }
        }
    }

    [HideInInspector]
    public bool isLastShotOver;

    //TODO: Move it outside
    [HideInInspector]
    public AudioSource As;

    [SerializeField, Header("Attacks"), Tooltip("Attack which will be triggered on LMB")]
    private InterfaceReference<IAttack> _primaryAttack;

    [SerializeField, Tooltip("Attack which will be triggered on RMB, it's not required")]
    private InterfaceReference<IAttack> _secondaryAttack;

    [SerializeField, Header("Varabiables to pass")]
    private WeaponDefinition weaponDefinition;

    private Rigidbody _ownerRigidbody;

    private void Awake()
    {
        As = GetComponent<AudioSource>();
        _ownerRigidbody = GetComponentInParent<Rigidbody>();

        MagazineSize = weaponDefinition.magazineSize;
        BulletsLeft = MagazineSize;
    }

    private void Start()
    {
        isLastShotOver = true;
    }

    public bool PrimaryAttack()
    {
        if (!CanShoot()) return false;
        return _primaryAttack.Value.Attack(this);
    }

    public float GetWeaponFireRate()
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

    public void SetOwner(GameObject inOwner)
    {
        owner = inOwner;
    }

    public void SetAmmo(int newAmmo)
    {
        BulletsLeft = newAmmo;
    }

    public GameObject GetWeapon()
    {
        return gameObject;
    }

    public Rigidbody GetRigidbodyOfWeaponOwner()
    {
        return _ownerRigidbody;
    }

    private bool CanShoot()
    {
        if(outOfAmmo)
        {
            if (owner != null)
            {
                if(owner.TryGetComponent(out WeaponsManager weaponsManager))
                {
                    weaponsManager.DestroyWeapon(weaponsManager.ActiveWeaponIndex);
                }
            }
            return false;
        }

        return isLastShotOver;
    }

    private void OnValidate()
    {
        Assert.IsNotNull(_primaryAttack.Value);
    }

    public void SetMagazineSize(int size)
    {
        MagazineSize = size;
    }

    public WeaponDefinition GetWeaponDefinition()
    {
        return weaponDefinition;
    }

    public float GetWeaponRange()
    {
        return Range;
    }
}
