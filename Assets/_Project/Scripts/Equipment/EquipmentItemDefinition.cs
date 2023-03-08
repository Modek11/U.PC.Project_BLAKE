using UnityEngine;

[CreateAssetMenu(fileName = "Equipment definition", menuName = "Project BLAKE/Equipment")]
public class EquipmentItemDefinition : ScriptableObject
{
    public GameObject equipmentPrefab;

    public string equipmentName;

    public string attachSocketName;

    public Vector3 locationOffset;
    
    public Quaternion rotation;

    public Vector3 scale;
}
