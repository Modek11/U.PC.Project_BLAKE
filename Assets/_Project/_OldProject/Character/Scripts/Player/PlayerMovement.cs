using UnityEngine;

[RequireComponent(typeof(PlayerInputController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    
    [SerializeField] private float playerSpeed;
    private PlayerInputController playerInputController;

    private Vector2 movementAxis;
    private Vector3 _playerInput;
    private Rigidbody _rigidbody;
    
    private Vector3 _mousePosition;
    private Vector3 _lookDirection;


    private void Awake()
    {
        playerInputController = GetComponent<PlayerInputController>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        playerInputController.movementEvent += MovementHandler;
    }

    private void Update()
    {
        Rotation();
        
    }


   void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovementHandler(Vector2 dir)
    {
        movementAxis = dir;
    }

    private void MovePlayer()
    {
        //Player movement
        movementAxis = movementAxis.normalized;
        _rigidbody.MovePosition(transform.position + (new Vector3(movementAxis.x,0, movementAxis.y) * (playerSpeed * Time.fixedDeltaTime)));
    }


   private void Rotation()
   {
       //Player rotation with mouse
       Ray rayCast = cam.ScreenPointToRay(Input.mousePosition);

       RaycastHit hit;

       if (Physics.Raycast(rayCast, out hit, 100))
       {
           _mousePosition = hit.point;
            
       }

       var position = transform.position;
       _lookDirection = _mousePosition - position;
       _lookDirection.y = 0;
        
       transform.LookAt(position + _lookDirection,Vector3.up);
   }

}
