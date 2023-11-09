using MBT;
using System.Collections;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Tasks/SetCombatState")]
public class BTT_SetCombatState : Leaf
{
    public CombatStateReference CombatStateReference = new CombatStateReference();
    public CombatState NewCombatState = CombatState.Strafe;

    public override NodeResult Execute()
    {
        StartCoroutine("SetNewValue");
        return NodeResult.success;
    }

    private IEnumerator SetNewValue()
    {
        yield return new WaitForEndOfFrame();

        CombatStateReference.GetVariable().Value = NewCombatState;
    }
}
