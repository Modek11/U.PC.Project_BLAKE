using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const string DIRECTION = "Direction";
    private const string SPEED = "Speed";
    
    [SerializeField] 
    private float playerSpeed;

    [SerializeField] 
    private float dashForce;

    [SerializeField] 
    private float dashCooldown;
    
    [SerializeField] 
    private float dashDuration;

    [SerializeField] 
    private Transform gunHandlerTransform;

    private Vector2 movementAxis;
    private Vector2 mousePosition;
    private float angleRotationDifference;
    private Rigidbody rigidbodyCache;
    private Animator animator;

    private float dashCooldownCountdown;
    private float dashDurationCountdown;
    private bool dashPerformed;
    private float minVelocityMagnitude = 0.1f;

    private Camera mainCamera;
    public event Action OnDashPerformed;
    
    public float DashCooldown => dashCooldown;
    public float DashCooldownCountdown => dashCooldownCountdown;

    private void Awake()
    {
        rigidbodyCache = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

    }

    private void OnEnable()
    {
        StartCoroutine(SetMainCamera());
    }

    private void Start()
    {
        ReferenceManager.PlayerInputController.movementEvent += MovementHandler;
        ReferenceManager.PlayerInputController.mousePositionEvent += MousePositionHandler;
        ReferenceManager.PlayerInputController.dashEvent += Dash;
        ReferenceManager.BlakeHeroCharacter.onDeath += Die;
        ReferenceManager.BlakeHeroCharacter.onRespawn += Respawn;
    }

    private void Update()
    {
        Rotation();
        DashCountdown();

        animator.SetFloat(DIRECTION, BlakeAnimatorHelper.CalculateDirection(rigidbodyCache.velocity, transform));
        animator.SetFloat(SPEED, rigidbodyCache.velocity.magnitude);
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
    private void MousePositionHandler(Vector2 dir)
    {
        mousePosition = dir;
    }

    private void MovePlayer()
    {
        movementAxis = movementAxis.normalized;
        Vector3 direction = new Vector3(movementAxis.x, 0, movementAxis.y);
        Vector3 isometricDirection = direction.ToIsometric();
        
        rigidbodyCache.AddForce(isometricDirection * playerSpeed, ForceMode.VelocityChange);
    }
    
   private void Rotation()
   {
        if (mainCamera == null) return;
       Plane playerPlane = new Plane(Vector3.up, gunHandlerTransform.position);
       Ray ray = mainCamera.ScreenPointToRay(mousePosition);
       
       
       if (playerPlane.Raycast(ray, out var hitDistance))
       {
           Vector3 targetPoint = ray.GetPoint(hitDistance);
           Quaternion playerRotation = transform.rotation;
           Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position).normalized;
           targetRotation.x = 0;
           targetRotation.z = 0;

           if (!ShouldPlayerRotate(playerRotation, targetRotation)) return;
           rigidbodyCache.MoveRotation(Quaternion.Slerp(playerRotation, targetRotation, 2000f * Time.deltaTime).normalized);
       }
   }

   private bool ShouldPlayerRotate(Quaternion playerRotation, Quaternion targetRotation)
   {
       //without this function, rigidbody calculating smth all the time
       var angle = Quaternion.Angle(playerRotation, targetRotation);
       if (angleRotationDifference == angle) return false;
       
       angleRotationDifference = angle;
       return true;

   }

   private void Dash()
   {
       var rbVelocity = rigidbodyCache.velocity;

       if (dashCooldownCountdown > 0 || rbVelocity.magnitude < minVelocityMagnitude)
       {
           return;
       }

       var force = rigidbodyCache.velocity.normalized * playerSpeed * dashForce;
       rigidbodyCache.AddForce(force, ForceMode.Impulse);

       SetDashCountdowns();
   }
   
   private void SetDashCountdowns()
   {
       dashCooldownCountdown = dashCooldown;
       dashDurationCountdown = dashDuration;
       OnDashPerformed?.Invoke();
   }

   private void DashCountdown()
   {
       if (dashDurationCountdown > 0)
       {
           dashDurationCountdown -= Time.deltaTime;
       }

       if (dashCooldownCountdown > 0)
       {
           dashCooldownCountdown -= Time.deltaTime;
       }
   }

   private void SpeedControl()
   {
       if (dashDurationCountdown > 0) return;
       
       var rbVelocity = rigidbodyCache.velocity;

       if (rbVelocity.magnitude < minVelocityMagnitude)
       {
           rigidbodyCache.velocity = Vector3.zero;
           return;
       }
       
       
       Vector3 currentVelocity = new Vector3(rbVelocity.x, 0, rbVelocity.z);
       if (currentVelocity.magnitude > playerSpeed)
       {
           currentVelocity = currentVelocity.normalized * playerSpeed;
           rigidbodyCache.velocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
       }
   }
   
   private void Die()
    {
        ReferenceManager.PlayerInputController.enabled = false;
        this.enabled = false;
    }

    private void Respawn()
    {
        ReferenceManager.PlayerInputController.enabled = true;
        this.enabled = true;
    }

   private IEnumerator SetMainCamera()
   {
       while (Camera.main == null)
       {
           yield return new WaitForSeconds(0.1f);
       }

       mainCamera = Camera.main;
   }
}
