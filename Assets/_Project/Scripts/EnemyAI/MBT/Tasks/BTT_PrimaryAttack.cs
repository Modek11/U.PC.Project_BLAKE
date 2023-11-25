using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Tasks/Primary attack")]
public class BTT_PrimaryAttack : Leaf
{
    public CombatStateReference CombatStateReference;
    public AIController AIController;
    public float WeaponRangeMultipler = 1f;

    private float weaponRange;
    private Weapon weaponInterface;

    public override NodeResult Execute()
    {
        if (AIController == null)
        {
            AIController = GetComponent<AIController>();
            if (AIController == null) return NodeResult.failure;
        }
        if (weaponInterface == null)
        {
            weaponInterface = AIController?.GetWeaponRef().GetComponent<Weapon>();
            if (weaponInterface == null) return NodeResult.failure;
        }
        if (ReferenceManager.BlakeHeroCharacter == null) return NodeResult.failure;

        weaponInterface.PrimaryAttack();

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
