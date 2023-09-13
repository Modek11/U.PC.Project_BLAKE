using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedRoomObject : MonoBehaviour
{
    [SerializeField]
    private GameObject[] randomizedObjects;

    private bool isSpawned = false;

    public void InitializeRandomObject()
    {
        if(!isSpawned)
        {
            if(randomizedObjects.Length > 0)
            {
                Instantiate(randomizedObjects[Random.Range(0, randomizedObjects.Length)], gameObject.transform.position, Quaternion.identity, gameObject.transform);
                isSpawned = true;
            }
        }
    }
}
