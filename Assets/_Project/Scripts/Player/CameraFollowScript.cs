using _Project.Scripts;
using _Project.Scripts.GlobalHandlers;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    [SerializeField] private bool followPlayerWithMouseOffset = true;
    [SerializeField] private bool smoothCameraMovement = true;

    [Range(1,20)]
    [SerializeField] private float distanceBetweenMouseAndPlayer = 6;
    [Range(1,50)]
    [SerializeField] private float maxDistanceFromPlayer = 25;
    
    private Transform playerTransform;
    
    private Vector3 playerPos;
    private Vector2 _mousePosition;
    private Vector3 _mousePositionWorldPoint;
    private Camera _cam;

    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Start()
    {
        ReferenceManager.PlayerInputController.mousePositionEvent += MousePositionHandler;
    }

    private void LateUpdate()
    {
        if (followPlayerWithMouseOffset)
        {
            FollowPlayerWithMouseOffset();
        }
        else
        {
            transform.position = playerTransform.position;
        }
    }

    private void FollowPlayerWithMouseOffset()
    {
        if (playerTransform == null) return;
        playerPos = playerTransform.position;
        GetMousePositionWorldPoint();

        float distance = Vector3.Distance(playerPos, _mousePositionWorldPoint);
        distance = Mathf.Clamp(distance, 0,maxDistanceFromPlayer);
        distance /= distanceBetweenMouseAndPlayer;
        
        Vector3 targetDirection = _mousePositionWorldPoint - playerPos;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 1f, 0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
        Vector3 targetPosition = transform.forward * distance + playerPos;


        if (smoothCameraMovement)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.05f);
        }
        else
        {
            transform.position = targetPosition;
        }
    }

    private void MousePositionHandler(Vector2 dir)
    {
        _mousePosition = dir;
    }

    private void GetMousePositionWorldPoint()
    {
        Plane playerPlane = new Plane(Vector3.up,playerPos);
        Ray ray = _cam.ScreenPointToRay(_mousePosition);

        if (playerPlane.Raycast(ray, out var hitDistance))
        {
            _mousePositionWorldPoint = ray.GetPoint(hitDistance);
        }
    }

    public void SetPlayerReference(Transform player)
    {
        playerTransform = player;
    }
}
