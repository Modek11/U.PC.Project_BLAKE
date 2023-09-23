using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode("Services/Shoot")]
public class BTS_Shoot : Service
{
    public BoolReference HasLineOfSightReference;
    public EnemyAIManager AIController;
    private IWeapon weapon;

    public override void Task()
    {
        if (AIController == null) return;
        if(weapon == null)
        {
            weapon = AIController.GetWeaponRef()?.GetComponent<IWeapon>();
            if (weapon == null) return;
        }
        if (!HasLineOfSightReference.Value) return;

        weapon.PrimaryAttack();
    }
}
