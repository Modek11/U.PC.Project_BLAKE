using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private float playerOffset = 10f;

    // Update is called once per frame
    void Update()
    {
        if(playerTransform != null)
        {
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + playerOffset, playerTransform.position.z);
        }
    }

    public void SetPlayer(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }
}
