using UnityEngine;

[CreateAssetMenu (fileName = "New Gun", menuName = "Scriptable Objects/Gun")]
public class GunScriptableObject : ScriptableObject
{
    public byte index;
    public string nameToDisplay;
    public byte magazineSize;
    public byte bulletsLeft;
    public float rateOfFire;
    public float reloadTime;
    public GameObject bulletPrefab;
}
