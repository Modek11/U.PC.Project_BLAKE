using MBT;
using System;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Conditions/Is Combat State Equal")]
public class BTD_IsCombatStateEqual : Condition
{
    public Abort abort;
    public bool invert = false;
    public CombatStateReference combatStateReference = new CombatStateReference();
    public CombatState combatState;

    public override bool Check()
    {
        return combatState == combatStateReference.GetVariable().Value;
    }

    public override void OnAllowInterrupt()
    {
        if (abort != Abort.None)
        {
            ObtainTreeSnapshot();
            combatStateReference.GetVariable().AddListener(OnVariableChange);
        }
    }

    public override void OnDisallowInterrupt()
    {
        if (abort != Abort.None)
        {
            combatStateReference.GetVariable().RemoveListener(OnVariableChange);
        }
    }

    private void OnVariableChange(CombatState oldValue, CombatState newValue)
    {
        EvaluateConditionAndTryAbort(abort);
    }

    public override bool IsValid()
    {
        return !combatStateReference.isInvalid;
    }
}
