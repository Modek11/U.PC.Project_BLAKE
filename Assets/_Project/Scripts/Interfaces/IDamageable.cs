
using UnityEngine;

namespace _Project.Scripts.Interfaces
{
    public interface IDamageable
    {
        bool TryTakeDamage(GameObject instigator, int damage);
        bool CanTakeDamage(GameObject instigator);
    }
}
