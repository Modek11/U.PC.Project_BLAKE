using UnityEngine;

public class BlakeAnimatorHelper : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        if(_rigidbody == null)
        {
            Debug.LogError("Rigidbody is not valid");
            enabled = false;
        }

        if (animator == null)
        {
            Debug.LogError("Animator is not valid");
            enabled = false;
        }
    }

    void Update()
    {
        animator.SetFloat("Direction", CalculateDirection());
        animator.SetFloat("Speed", _rigidbody.velocity.magnitude);
    }

    float CalculateDirection()
    {
        if (_rigidbody.velocity != Vector3.zero)
        {
            Vector3 NormalizedVel = _rigidbody.velocity.normalized;

            float ForwardCosAngle = Vector3.Dot(transform.forward, NormalizedVel);
            float ForwardDeltaDegree = Mathf.Acos(ForwardCosAngle) * Mathf.Rad2Deg;

            float RightCosAngle = Vector3.Dot(transform.right, NormalizedVel);
            if (RightCosAngle < 0)
            {
                ForwardDeltaDegree *= -1;
            }

            return ForwardDeltaDegree;
        }

        return 0f;
    }
}
