using UnityEngine;

public class CylinderStretcher : MonoBehaviour
{
    [SerializeField] private FloorManager floorManager;
    private Transform playerTransform;
    private Transform mainCameraTransform;
    private Transform cameraFollowTransform;
    
    private Vector3 initialScale;

    private void Start()
    {
        initialScale = transform.localScale;
        floorManager.FloorGeneratorEnd += FloorManagerOnFloorGeneratorEnd;
    }

    private void FloorManagerOnFloorGeneratorEnd(Transform playerTransform, Transform cameraFollowTransform)
    {
        this.playerTransform = playerTransform;
        this.cameraFollowTransform = cameraFollowTransform;
        mainCameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (!playerTransform.hasChanged || !mainCameraTransform.hasChanged) return;


        Vector3 playerPosition = playerTransform.position;
        Vector3 cameraPositionOffset = mainCameraTransform.position - (cameraFollowTransform.position - playerPosition);

        float distance = Vector3.Distance(cameraPositionOffset, playerPosition);
        transform.localScale = new Vector3(initialScale.x, distance / 2f, initialScale.z);

        Vector3 middlePoint = (cameraPositionOffset + playerPosition) / 2f;
        transform.position = middlePoint;

        Vector3 rotationDirection = (playerPosition - cameraPositionOffset);
        transform.up = rotationDirection;

    }
}
