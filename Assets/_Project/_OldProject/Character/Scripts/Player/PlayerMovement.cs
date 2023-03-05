using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    
    [SerializeField] private float playerSpeed;
    private PlayerInputSystem _controls;
    private Vector3 _playerInput;
    private Rigidbody _rigidbody;
    
    private Vector3 _mousePosition;
    private Vector3 _lookDirection;


    private void Awake()
    {
        _controls = new PlayerInputSystem();
        _controls.Gameplay.Enable();
        _rigidbody = GetComponent<Rigidbody>();
    }

   private void Update()
    {
        Rotation();
        
        //Player movement input
        _playerInput = _controls.Gameplay.Movement.ReadValue<Vector3>();
    }


   void FixedUpdate()
    {
        //Player movement
        _rigidbody.MovePosition(transform.position + (_playerInput * (playerSpeed * Time.fixedDeltaTime)));
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

   private void OnDestroy()
   {
       _controls.Gameplay.Disable();
   }
}
