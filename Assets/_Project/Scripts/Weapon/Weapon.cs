using AYellowpaper;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour, IWeapon
{
    //TODO: Implement range
    public float Range;
    public int MagazineSize;
    public Transform BulletsSpawnPoint;
    public GameObject BulletPrefab;
    [HideInInspector] public bool isLastShotOver;
    public int BulletsLeft = 10;
    //TODO: Move it outside
    [HideInInspector] public AudioSource As;
    [Header("Attacks")]
    [Tooltip("Attack which will be triggered on LMB")]
    [SerializeField] private InterfaceReference<IAttack> _primaryAttack;
    [Tooltip("Attack which will be triggered on RMB, it's not required")]
    [SerializeField] private InterfaceReference<IAttack> _secondaryAttack;
    [Header("Varabiables to pass")]
    [SerializeField] private WeaponDefinition weaponDefinition;
    private Rigidbody _ownerRigidbody;

    private void Awake()
    {
        As = GetComponent<AudioSource>();
        _ownerRigidbody = GetComponentInParent<Rigidbody>();
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
    
    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public Rigidbody GetRigidbodyOfWeaponOwner()
    {
        return _ownerRigidbody;
    }

    private bool CanShoot()
    {
        return isLastShotOver && BulletsLeft > 0;
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
}
