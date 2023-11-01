using DG.Tweening.Core.Easing;
using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Tasks/FindPositionAroundTarget")]
public class BTT_FindPositionAroundTarget : Leaf
{
    public AIController AIController;
    public Vector3Reference vector3Reference;

    public override NodeResult Execute()
    {
        if (AIController == null) return NodeResult.failure;

        Vector3 offset = Vector3.zero;

        if (AIController.GetWeaponRef().name != "Baton")
        {
            offset = Random.insideUnitSphere * 5f;
            offset = new Vector3(offset.x, 0, offset.z);
        }

        vector3Reference.Value = ReferenceManager.BlakeHeroCharacter.transform.position + offset;
        return NodeResult.success;
    }
}
