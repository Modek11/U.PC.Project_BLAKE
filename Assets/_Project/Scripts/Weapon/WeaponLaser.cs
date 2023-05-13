using UnityEngine;

public class WeaponLaser : MonoBehaviour
{
    [SerializeField] private Transform _attackPointTransform;

    private IWeapon _weaponInterface;
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _weaponInterface = GetComponentInParent<Weapon>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        Vector3 _laserLength = new Vector3(0f, 0f, _weaponInterface.GetWeaponRange() - 2f);

        transform.position = _attackPointTransform.position;
        _lineRenderer.SetPosition(1, _laserLength);   
    }

}
