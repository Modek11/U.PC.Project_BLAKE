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
    
    //TODO: Move this to place where weapon will be handled
    [Tooltip("If marked you can just hold button to shoot (like AK)")]
    [SerializeField] private bool allowButtonHold;
    
    [Header("Attacks")]
    [Tooltip("Attack which will be triggered on LMB")]
    [SerializeField] private InterfaceReference<IAttack> _primaryAttack;
    [Tooltip("Attack which will be triggered on RMB, it's not required")]
    [SerializeField] private InterfaceReference<IAttack> _secondaryAttack;
    
    //TODO: Remove these variables after connecting weapons with player controller
    private bool isPlayerTryingShooting;
    private bool isPlayerTryingShooting2;
    
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

    private void Update()
    {
        TempInput();
    }

    //TODO: It will be handled in another place, remove this after that
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
        
        _primaryAttack.Value.Attack(this);
    }
    
    public void SecondaryAttack()
    {
        if (_secondaryAttack.Value is null) return;
        if (!CanShoot(isPlayerTryingShooting2)) return;
        
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
    
    private bool CanShoot(bool isShotButtonPressed)
    {
        return isLastShotOver && isShotButtonPressed && !isReloading && BulletsLeft > 0;
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
