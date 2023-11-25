using System;

using UnityEngine;
using UnityEditor;

using GameFramework.System.Attributes;
using GameFramework.System;

namespace GameFramework.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(StateAttribute))]
    public class StateDrawer : PropertyDrawer
    {
        string[] StringList = null;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (StringList != StatesManager.StatesNames) StringList = StatesManager.StatesNames;

            if (StringList != null && StringList.Length != 0)
            {
                SerializedProperty SerializedProperty = property.FindPropertyRelative("StateName");

                int SelectedIndex = Mathf.Max(Array.IndexOf(StringList, SerializedProperty.stringValue), 0);
                SelectedIndex = EditorGUI.Popup(position, property.name, SelectedIndex, StringList);
                SerializedProperty.stringValue = StringList[SelectedIndex];
            }
            else EditorGUI.PropertyField(position, property, label);
        }
    }
}