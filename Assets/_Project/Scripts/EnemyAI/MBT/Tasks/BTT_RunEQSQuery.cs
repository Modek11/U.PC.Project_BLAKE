using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Tasks/Run EQS query")]
public class BTT_RunEQSQuery : Leaf
{
    public EnvQuery EQSQuery;
    public Vector3Reference AimLocationReference;

    public override void OnEnter()
    {
        EQSQuery.PrepareQuery();
    }

    public override NodeResult Execute()
    {
        if (EQSQuery == null) return NodeResult.failure;

        EQSQuery.ProgressQuery();
        
        if (EQSQuery != null && EQSQuery.QueryStatus == EQSStatus.Finished)
        {
            if (EQSQuery.BestResult == null) return NodeResult.failure;

            AimLocationReference.Value = EQSQuery.BestResult.GetWorldPosition();
            return NodeResult.success;
        }

        return NodeResult.running;
    }
}
