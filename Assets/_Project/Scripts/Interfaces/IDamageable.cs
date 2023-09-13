
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(GameObject instigator, int damage);
    bool CanTakeDamage(GameObject instigator);
}
