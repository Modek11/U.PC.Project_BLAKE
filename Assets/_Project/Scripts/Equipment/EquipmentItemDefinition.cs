using UnityEngine;

[CreateAssetMenu(fileName = "Equipment definition", menuName = "Project BLAKE/Equipment")]
public class EquipmentItemDefinition : ScriptableObject
{
    public GameObject equipmentPrefab;

    public string equipmentName;

    public string boneName;

    public Vector3 locationOffset;
    
    public Quaternion rotationOffset;

    public Vector3 scale;
}
