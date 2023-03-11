using UnityEngine;

[RequireComponent(typeof(PlayerInputController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    
    [SerializeField] private float playerSpeed;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashDuration;


    private PlayerInputController playerInputController;

    private Vector2 movementAxis;
    private Vector2 mousePosition;
    private Vector3 _playerInput;
    private Rigidbody _rigidbody;
    
    private Vector3 _mousePosition;
    private Vector3 _lookDirection;
    
    private float _dashCooldownCountdown;
    private float _dashDurationCountdown;

    private bool _dashPerformed;


    private void Awake()
    {
        playerInputController = GetComponent<PlayerInputController>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        playerInputController.movementEvent += MovementHandler;
        playerInputController.rotationEvent += RotationHandler;
        playerInputController.dashEvent += Dash;
    }

    private void Update()
    {
        Rotation();
        DashCountdown();
    }


   void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
    }

    private void MovementHandler(Vector2 dir)
    {
        movementAxis = dir;
    }
    private void RotationHandler(Vector2 dir)
    {
        mousePosition = dir;
    }

    private void MovePlayer()
    {
        //Player movement
        movementAxis = movementAxis.normalized;
        
        _rigidbody.AddForce(new Vector3(movementAxis.x,0, movementAxis.y) * (playerSpeed * 10f), ForceMode.Force);
    }
    
   private void Rotation()
   {
       //Player rotation with mouse
       Ray rayCast = cam.ScreenPointToRay(mousePosition);

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

   private void Dash()
   {
       if (_dashCooldownCountdown > 0) return;
       
       _rigidbody.AddForce(_rigidbody.velocity * dashForce, ForceMode.Impulse);

       _dashPerformed = true;
   }

   private void SpeedControl()
   {
       if (_dashDurationCountdown > 0) return;
       
       var rbVelocity = _rigidbody.velocity;
       
       Vector3 currentVelocity = new Vector3(rbVelocity.x, 0, rbVelocity.z);
       if (currentVelocity.magnitude > playerSpeed)
       {
           currentVelocity = currentVelocity.normalized * playerSpeed;
           _rigidbody.velocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
       }
   }

   private void DashCountdown()
   {
       if (_dashDurationCountdown > 0)
       {
           _dashDurationCountdown -= Time.deltaTime;
       }

       if (_dashCooldownCountdown > 0)
       {
           _dashCooldownCountdown -= Time.deltaTime;
       }

       if (_dashPerformed)
       {
           _dashDurationCountdown = dashDuration;
           _dashCooldownCountdown = dashCooldown;
           _dashPerformed = false;
       }
   }

}
