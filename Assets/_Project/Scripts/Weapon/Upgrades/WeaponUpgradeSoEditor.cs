using UnityEditor;

namespace _Project.Scripts.Weapon.Upgrades
{
    [CustomEditor(typeof(WeaponUpgradeSo))]
    public class WeaponUpgradeSoEditor : Editor
    {
        SerializedProperty weaponDefinition;
        SerializedProperty rangedUpgradeData;
        SerializedProperty meleeUpgradeData;

        void OnEnable()
        {
            weaponDefinition = serializedObject.FindProperty("weaponDefinition");
            rangedUpgradeData = serializedObject.FindProperty("rangedUpgradeData");
            meleeUpgradeData = serializedObject.FindProperty("meleeUpgradeData");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(weaponDefinition);

            if (weaponDefinition.objectReferenceValue != null)
            {
                if (weaponDefinition.objectReferenceValue is RangedWeaponDefinition)
                {
                    EditorGUILayout.PropertyField(rangedUpgradeData, true);
                }
                else
                {
                    EditorGUILayout.PropertyField(meleeUpgradeData, true);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("WeaponDefinition is not assigned.", MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}