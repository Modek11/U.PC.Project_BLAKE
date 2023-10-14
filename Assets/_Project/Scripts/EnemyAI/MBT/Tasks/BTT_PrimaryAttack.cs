using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Tasks/Primary attack")]
public class BTT_PrimaryAttack : Leaf
{
    public CombatStateReference CombatStateReference;
    public EnemyAIManager AIController;
    public float WeaponRangeMultipler = 1f;

    private float weaponRange;
    private IWeapon weaponInterface;

    public override NodeResult Execute()
    {
        if (AIController == null)
        {
            AIController = GetComponent<EnemyAIManager>();
            if (AIController == null) return NodeResult.failure;
        }
        if (weaponInterface == null)
        {
            weaponInterface = AIController?.GetWeaponRef().GetComponent<IWeapon>();
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
