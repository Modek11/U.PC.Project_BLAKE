using UnityEngine;

[CreateAssetMenu(fileName = "Weapon definition", menuName = "Project BLAKE/Weapon")]
public class WeaponDefinition : ScriptableObject
{
    public GameObject weaponPrefab;

    public string weaponName;

    public string attachSocketName;

    public Vector3 locationOffset;
    
    public Quaternion rotation;

    public Vector3 scale;
}
