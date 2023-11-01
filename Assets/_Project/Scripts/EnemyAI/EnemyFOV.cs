using System.Collections;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public float Radius;
    [Range(0, 360)] public float Angle;

    public LayerMask TargetMask;
    public LayerMask ObstacleMask;

    [HideInInspector]
    public bool CanSeePlayer;

    public delegate void CanSeePlayerChanged(bool newCanSeePlayer);
    public event CanSeePlayerChanged OnCanSeePlayerChanged;

    private void OnEnable()
    {
        StartCoroutine(FOVRoutine());
    }

    public void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, Radius, TargetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < Angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position + new Vector3(0, 3f, 0), directionToTarget, distanceToTarget, ObstacleMask))
                {
                    if (!CanSeePlayer)
                    {
                        CanSeePlayer = true;
                        OnCanSeePlayerChanged?.Invoke(CanSeePlayer);
                    }
                    return;
                }
            }
        }
        if (CanSeePlayer)
        {
            CanSeePlayer = false;
            OnCanSeePlayerChanged?.Invoke(CanSeePlayer);
        }

    }

    private IEnumerator FOVRoutine()
    {
        float fovCheckDelay = 0.2f;

        while (true)
        {
            yield return new WaitForSeconds(fovCheckDelay);

            FieldOfViewCheck();
        }
        
    }
}
