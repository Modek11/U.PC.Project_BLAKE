using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Weapon definition", menuName = "Project BLAKE/Weapon")]
public class WeaponDefinition : ScriptableObject
{
    public GameObject weaponPrefab;

    public string weaponName;

    public int magazineSize;

    public string attachSocketName;

    public Vector3 locationOffset;
    
    public Quaternion rotation;

    public Vector3 scale = Vector3.one;

    public GameObject weaponGFX;

    public Vector3 pickupLocationOffset = Vector3.zero;

    public Quaternion pickupRotation;
}
