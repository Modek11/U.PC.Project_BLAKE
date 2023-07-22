using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    private Collider[] _ragdollColliders;
    private Rigidbody[] _ragdollRigidbodies;

    private BlakeCharacter _eventSubscribe;
    private CapsuleCollider _characterCollider;

    private void Awake()
    {
        SetupRagdoll();
    }

    private void Start()
    {
        DisableRagdoll();
    }

    private void OnEnable()
    {
        _eventSubscribe.onDeath += EnableRagdoll;
    }

    private void OnDestroy()
    {
        _eventSubscribe.onDeath -= EnableRagdoll;
    }

    private void EnableRagdoll()
    {
        animator.enabled = false;

        for (int i = 0; i < _ragdollColliders.Length; i++)
        {
            _ragdollColliders[i].enabled = true;
        }

        for (int i = 0; i < _ragdollRigidbodies.Length; i++)
        {
            _ragdollRigidbodies[i].isKinematic = false;
        }

        _characterCollider.enabled = false;
    }

    private void DisableRagdoll()
    {
        animator.enabled = true;

        for (int i = 0; i < _ragdollColliders.Length; i++)
        {
            _ragdollColliders[i].enabled = false;
        }

        for (int i = 0; i < _ragdollRigidbodies.Length; i++)
        {
            _ragdollRigidbodies[i].isKinematic = true;
        }

        _characterCollider.enabled = true;
    }

    private void SetupRagdoll()
    {
        _eventSubscribe = GetComponent<BlakeCharacter>();
        _characterCollider = GetComponent<CapsuleCollider>();
        _ragdollColliders = GetComponentsInChildren<Collider>();
        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
    }

   


}
