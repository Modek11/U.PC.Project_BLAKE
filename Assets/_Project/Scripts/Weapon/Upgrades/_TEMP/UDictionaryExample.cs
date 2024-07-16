using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class UDictionaryExample : MonoBehaviour
{
    [InitializeOnLoadMethod]
    static void OnLoad()
    {
        AudioListener.volume = 0.2f;
    }

    [UDictionary.Split(30, 70)]
    public UDictionary1 dictionary1;
    [Serializable]
    public class UDictionary1 : UDictionary<string, string> { }

    [UDictionary.Split(50, 50)]
    public UDictionary2 dictionary2;
    [Serializable]
    public class UDictionary2 : UDictionary<Key, Value> { }

    [UDictionary.Split(30, 70)]
    public UDictionary3 dictionary3;
    [Serializable]
    public class UDictionary3 : UDictionary<Component, Vector3> { }

    [Serializable]
    public class Key
    {
        public string id;

        public string file;
    }

    [Serializable]
    public class Value
    {
        public string firstName;

        public string lastName;
    }

    void Start()
    {
        dictionary1["See Ya Later"] = "Space Cowboy";
    }
}