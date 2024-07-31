using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.EnemyAI
{
    public class EnemyFOV : MonoBehaviour
    {
        public float Radius;
        [Range(0, 360)] public float Angle;

        [SerializeField] private LayerMask TargetMask;
        [SerializeField] private LayerMask ObstacleMask;
        [SerializeField] private Transform raycastOrigin;
        [SerializeField] private float fovCheckDelay;
        [SerializeField] private float maxBackVisibilityDistance;
        [SerializeField] private float findPlayerFromBehindDelay;
        

        //create one Collider because there will be only one player
        private Collider[] overlapSphereColliders = new Collider[1];

        [HideInInspector]
        public bool CanSeePlayer;

        public delegate void CanSeePlayerChanged(bool newCanSeePlayer);
        public event CanSeePlayerChanged OnCanSeePlayerChanged;

        private void OnEnable()
        {
            _ = FOVRoutine();
        }
        
        private async UniTaskVoid FOVRoutine()
        {
            var cToken = this.GetCancellationTokenOnDestroy();
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(fovCheckDelay), cancellationToken: cToken);
                _ = FieldOfViewCheck(cToken);
            }
        }

        private async UniTaskVoid FieldOfViewCheck(CancellationToken cToken)
        {
            Physics.OverlapSphereNonAlloc(transform.position, Radius, overlapSphereColliders, TargetMask);

            if (overlapSphereColliders[0] is null)
            {
                if (CanSeePlayer)
                {
                    OnCanSeePlayerChanged?.Invoke(CanSeePlayer = false);
                }
                
                return;
            }

            if (CanSeePlayer)
            {
                return;
            }

            var target = overlapSphereColliders[0].transform;
            var directionToTarget = (target.position - transform.position).normalized;
            var distanceToTarget = Vector3.Distance(transform.position, target.position);
            
            if (!Physics.Raycast(raycastOrigin.position, directionToTarget, distanceToTarget, ObstacleMask))
            {
                //front of enemy 
                if (Vector3.Angle(transform.forward, directionToTarget) < Angle / 2)
                {
                    OnCanSeePlayerChanged?.Invoke(CanSeePlayer = true);
                }
                //back of enemy
                else if(distanceToTarget < maxBackVisibilityDistance)
                {
                    CanSeePlayer = true;
                    await UniTask.Delay(TimeSpan.FromSeconds(findPlayerFromBehindDelay), cancellationToken: cToken);
                    OnCanSeePlayerChanged?.Invoke(CanSeePlayer);
                }
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Player"))
            {
                OnCanSeePlayerChanged?.Invoke(CanSeePlayer = true);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, maxBackVisibilityDistance);
            Gizmos.DrawWireSphere(transform.position, Radius);
        }
#endif
    }
}
