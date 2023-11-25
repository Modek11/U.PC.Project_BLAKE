using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using GameFramework.System.Attributes;

namespace GameFramework.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(MonoScriptAttribute), false)]
    public class MonoScriptPropertyDrawer : PropertyDrawer
    {
        static Dictionary<string, MonoScript> ScriptCache;
        static MonoScriptPropertyDrawer()
        {
            ScriptCache = new Dictionary<string, MonoScript>();
            var Scripts = Resources.FindObjectsOfTypeAll<MonoScript>();
            for (int i = 0; i < Scripts.Length; i++)
            {
                var Type = Scripts[i].GetClass();
                if (Type != null && !ScriptCache.ContainsKey(Type.FullName))
                {
                    ScriptCache.Add(Type.FullName, Scripts[i]);
                }
            }
        }
        bool ViewString = false;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                Rect R = EditorGUI.PrefixLabel(position, label);
                Rect LabelRect = position;
                LabelRect.xMax = R.xMin;
                position = R;
                ViewString = GUI.Toggle(LabelRect, ViewString, "", "label");
                if (ViewString)
                {
                    property.stringValue = EditorGUI.TextField(position, property.stringValue);
                    return;
                }
                MonoScript Script = null;
                string typeName = property.stringValue;
                if (!string.IsNullOrEmpty(typeName))
                {
                    ScriptCache.TryGetValue(typeName, out Script);
                    if (Script == null)
                        GUI.color = Color.red;
                }

                Script = (MonoScript)EditorGUI.ObjectField(position, Script, typeof(MonoScript), false);
                if (GUI.changed)
                {
                    if (Script != null)
                    {
                        var Type = Script.GetClass();
                        MonoScriptAttribute attr = (MonoScriptAttribute)attribute;
                        if (attr.Type != null && !attr.Type.IsAssignableFrom(Type))
                            Type = null;
                        if (Type != null)
                            property.stringValue = Type.FullName;
                        else
                            Debug.LogWarning("The script file " + Script.name + " doesn't contain an assignable class");
                    }
                    else
                        property.stringValue = "";
                }
            }
            else
            {
                GUI.Label(position, "The MonoScript attribute can only be used on string variables");
            }
        }
    }
}