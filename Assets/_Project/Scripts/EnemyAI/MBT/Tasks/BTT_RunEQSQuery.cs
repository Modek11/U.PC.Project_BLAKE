using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Tasks/Run EQS query")]
public class BTT_RunEQSQuery : Leaf
{
    public EnvQuery EQSQuery;
    public Vector3Reference AimLocationReference;

    public override NodeResult Execute()
    {
        if (EQSQuery == null) return NodeResult.failure;

        EQSQuery.RunEQSQuery();

        if (EQSQuery != null && EQSQuery.BestResult != null)
        {
            AimLocationReference.Value = EQSQuery.BestResult.GetWorldPosition();
            return NodeResult.success;
        }

        return NodeResult.running;
    }
}
