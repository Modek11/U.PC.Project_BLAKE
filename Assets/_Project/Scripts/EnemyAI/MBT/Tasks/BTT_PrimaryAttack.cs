using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Tasks/Primary attack")]
public class BTT_PrimaryAttack : Leaf
{
    public CombatStateReference CombatStateReference;
    public AIController AIController;
    public float WeaponRangeMultipler = 1f;

    public override NodeResult Execute()
    {
        if (AIController == null)
        {
            AIController = GetComponent<AIController>();
            if (AIController == null) return NodeResult.failure;
        }
        if (AIController.Weapon == null) return NodeResult.failure;
        if (ReferenceManager.BlakeHeroCharacter == null) return NodeResult.failure;
        if (!AIController.Weapon.CanPrimaryAttack()) return NodeResult.failure;
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(AIController.transform.position);
        if (viewportPosition.x <= 0 || viewportPosition.x >= 1 || viewportPosition.y <= 0 || viewportPosition.y >= 1) return NodeResult.failure;

        AIController.Weapon.PrimaryAttack();

        //float distanceToPlayer = Vector3.Distance(AIController.transform.position, ReferenceManager.BlakeHeroCharacter.transform.position);

        //if (distanceToPlayer <= weaponRange * WeaponRangeMultipler)
        //{
        //    AIController.navMeshAgent.isStopped = true;
        //}

        //if (distanceToPlayer > weaponRange)
        //{
        //    CombatStateReference.GetVariable().Value = CombatState.Chase;
        //}

        return NodeResult.success;
    }
}
