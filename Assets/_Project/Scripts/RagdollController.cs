using UnityEngine;
using UnityEngine.AI;

public class RagdollController : MonoBehaviour
{
    [SerializeField] 
    private Animator animator;

    [SerializeField] 
    private NavMeshAgent agent;

    [SerializeField]
    private CapsuleCollider mainCollider;

    [SerializeField]
    private Collider[] ragdollColliders;

    private Rigidbody[] _ragdollRigidbodies;

    private BlakeCharacter _eventSubscribe;

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
        agent.enabled = false;
        mainCollider.enabled = false;

        for (int i = 0; i < ragdollColliders.Length; i++)
        {
            ragdollColliders[i].enabled = true;
        }
        

        for (int i = 0; i < _ragdollRigidbodies.Length; i++)
        {
            _ragdollRigidbodies[i].useGravity = true;
            _ragdollRigidbodies[i].isKinematic = false;
        }
    }

    private void DisableRagdoll()
    {
        animator.enabled = true;
        agent.enabled = true;

        for (int i = 0; i < _ragdollRigidbodies.Length; i++)
        {
            _ragdollRigidbodies[i].useGravity = false;
            _ragdollRigidbodies[i].isKinematic = true;
        }
    }

    private void SetupRagdoll()
    {
        _eventSubscribe = GetComponent<BlakeCharacter>();
        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
    }
   


}
