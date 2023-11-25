using UnityEngine;

public class WeaponLaser : MonoBehaviour
{
    [SerializeField]
    private Transform attackPointTransform;

    private RangedWeapon rangedWeapon;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        rangedWeapon = GetComponentInParent<RangedWeapon>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        Vector3 _laserLength = new Vector3(0f, 0f, rangedWeapon.Range - 2f);

        transform.position = attackPointTransform.position;
        lineRenderer.SetPosition(1, _laserLength);   
    }
}
