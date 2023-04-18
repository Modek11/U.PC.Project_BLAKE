using UnityEngine;
using Random = UnityEngine.Random;

public class SniperAttack : MonoBehaviour, IAttack
{
    [Tooltip("Declares how often player can shoot")]
    [SerializeField] private float timeBetweenShooting;
    [Tooltip("Declares range of bullet spawn, while player is moving")]
    [SerializeField] private float spread;
    private Weapon usedWeapon;
    private PlayerInputController playerInputController;
    private Vector2 movementAxis;

    //TODO: Change this if we'll have reference manager
    private void Awake()
    {
        playerInputController = FindObjectOfType<PlayerInputController>();
    }

    private void OnEnable()
    {
        playerInputController.movementEvent += OnMovementEvent;
    }

    private void OnDisable()
    {
        playerInputController.movementEvent -= OnMovementEvent;
    }

    public void Attack(Weapon weapon)
    {
        usedWeapon = weapon;
        Shot();
    }
    
    private void Shot()
    {
        usedWeapon.isLastShotOver = false;
        usedWeapon.As.PlayOneShot(usedWeapon.As.clip);
        
        //Choose spread depending on player's controls
        float chosenSpread = movementAxis == Vector2.zero ? 0 : Random.Range(-spread, spread);
        
        Instantiate(usedWeapon.BulletPrefab, usedWeapon.BulletsSpawnPoint.position, usedWeapon.transform.rotation).GetComponent<IBullet>().SetupBullet(chosenSpread, usedWeapon.transform.parent.gameObject);
        
        usedWeapon.BulletsLeft--;
        usedWeapon.Invoke(nameof(usedWeapon.ResetShot), timeBetweenShooting);
    }

    private void OnMovementEvent(Vector2 dir)
    {
        movementAxis = dir;
    }

    public float ReturnFireRate()
    {
        throw new System.NotImplementedException();
    }
}
