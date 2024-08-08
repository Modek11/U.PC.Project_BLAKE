using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.EditorDebug
{
    public class URPDebugRemover : MonoBehaviour
    {
        private void Start()
        {
            OnEnable();
        }

        private void OnEnable()
        {
            var fieldInfo = typeof(UnityEngine.Rendering.DebugManager).GetField("debugActionMap",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var debugActionMap = (InputActionMap)fieldInfo.GetValue(UnityEngine.Rendering.DebugManager.instance);
            debugActionMap.Disable();
        }
    }
}
