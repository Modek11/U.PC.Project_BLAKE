using System;

using UnityEngine;

namespace GameFramework.System.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class MonoScriptAttribute : PropertyAttribute
    {
        public Type Type;
    }

    public class DraggablePoint : PropertyAttribute { }

    public class StateAttribute : PropertyAttribute { }
}