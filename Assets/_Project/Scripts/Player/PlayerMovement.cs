using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Camera cam;
    
    [SerializeField] private float playerSpeed;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashDuration;

    [SerializeField] private Transform gunHandlerTransform;

    private Vector2 _movementAxis;
    private Vector2 _mousePosition;
    private float _angleRotationDifference;
    private Rigidbody _rigidbody;
    private Animator _animator;

    private float _dashCooldownCountdown;
    private float _dashDurationCountdown;
    private bool _dashPerformed;
    private GameObject _dashCooldownUI;
    private Image _cooldownImage;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();

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

        _animator.SetFloat("Direction", BlakeAnimatorHelper.CalculateDirection(_rigidbody.velocity, transform));
        _animator.SetFloat("Speed", _rigidbody.velocity.magnitude);
    }


   void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
    }

    private void MovementHandler(Vector2 dir)
    {
        _movementAxis = dir;
    }
    private void MousePositionHandler(Vector2 dir)
    {
        _mousePosition = dir;
    }

    private void MovePlayer()
    {
        _movementAxis = _movementAxis.normalized;
        Vector3 direction = new Vector3(_movementAxis.x, 0, _movementAxis.y);
        Vector3 isometricDirection = direction.ToIsometric();
        
        _rigidbody.AddForce(isometricDirection * (playerSpeed * 10f), ForceMode.Force);
    }
    
   private void Rotation()
   {
       Plane playerPlane = new Plane(Vector3.up, gunHandlerTransform.position);
       Ray ray = cam.ScreenPointToRay(_mousePosition);
       
       
       if (playerPlane.Raycast(ray, out var hitDistance))
       {
           Vector3 targetPoint = ray.GetPoint(hitDistance);
           Quaternion playerRotation = transform.rotation;
           Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position).normalized;
           targetRotation.x = 0;
           targetRotation.z = 0;

           if (!ShouldPlayerRotate(playerRotation, targetRotation)) return;
           _rigidbody.MoveRotation(Quaternion.Slerp(playerRotation, targetRotation, 20f * Time.deltaTime).normalized);
        }
   }

   private bool ShouldPlayerRotate(Quaternion playerRotation, Quaternion targetRotation)
   {
       //without this function, rigidbody calculating smth all the time
       var angle = Quaternion.Angle(playerRotation, targetRotation);
       if (_angleRotationDifference == angle) return false;
       
       _angleRotationDifference = angle;
       return true;

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
           _cooldownImage.fillAmount += Time.deltaTime;
           _dashCooldownUI.transform.position = _rigidbody.transform.position + Vector3.up * 0.6f;
           _dashCooldownUI.transform.LookAt(Camera.main.transform);
       }
       else
       {
           _dashCooldownUI.SetActive(false);
           _cooldownImage.fillAmount = 0;
       }

       if (_dashPerformed)
       {
           _dashCooldownUI.SetActive(true);
           _dashDurationCountdown = dashDuration;
           _dashCooldownCountdown = dashCooldown;
           _dashPerformed = false;
       }
   }

    private void Die()
    {
        ReferenceManager.PlayerInputController.enabled = false;
        _dashCooldownUI.SetActive(false);
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

       cam = Camera.main;
   }

    public void SetDashCooldownUIReference(GameObject dashCooldownUIReference)
    {
        _dashCooldownUI = dashCooldownUIReference;
        _cooldownImage = _dashCooldownUI.transform.GetChild(1).GetComponent<Image>();
    }
}
