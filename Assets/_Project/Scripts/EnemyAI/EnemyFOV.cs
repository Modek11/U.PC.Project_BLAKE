using System.Collections;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public GameObject playerRef;

    public float radius;
    [Range(0, 360)] public float angle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public bool canSeePlayer;

    public delegate void OnCanSeePlayerChanged(bool newCanSeePlayer);
    public event OnCanSeePlayerChanged onCanSeePlayerChanged;

    private void Awake()
    {
        FindPlayer();
    }

    public void FindPlayer()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        StartCoroutine(FOVRoutine());
    }

    public void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position + new Vector3(0, 3f, 0), directionToTarget, distanceToTarget, obstacleMask))
                {
                    if (!canSeePlayer)
                    {
                        canSeePlayer = true;
                        onCanSeePlayerChanged.Invoke(canSeePlayer);
                    }
                    return;
                }
            }
        }
        if (canSeePlayer)
        {
            canSeePlayer = false;
            onCanSeePlayerChanged.Invoke(canSeePlayer);
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
