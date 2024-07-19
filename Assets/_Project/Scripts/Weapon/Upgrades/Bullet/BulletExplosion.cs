using _Project.Scripts.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Weapon
{
    public class BulletExplosion : MonoBehaviour
    {
        [SerializeField]
        private float explosionRadius;

        [SerializeField] 
        private ParticleSystem particles;

        public void Explode(GameObject instigator)
        {
            Destroy(gameObject, particles.main.duration);
            particles.Play();
            
            var colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (var collider in colliders)
            {
                if (collider.transform.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable?.TryTakeDamage(instigator, 1);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
