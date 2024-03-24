using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode("Services/Shoot")]
public class BTS_Shoot : Service
{
    public BoolReference HasLineOfSightReference;
    public AIController AIController;

    public override void Task()
    {
        if (AIController == null) return;
        if (AIController.Weapon == null) return;
        if (!AIController.Weapon.CanPrimaryAttack()) return;
        if (!HasLineOfSightReference.Value) return;
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(AIController.transform.position);
        if (viewportPosition.x <= 0 || viewportPosition.x >= 1 || viewportPosition.y <= 0 || viewportPosition.y >= 1) return;

        AIController.Weapon.PrimaryAttack();
    }
}
