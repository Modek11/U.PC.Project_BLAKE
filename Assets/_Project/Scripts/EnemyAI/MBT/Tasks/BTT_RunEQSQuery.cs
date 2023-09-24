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
        EQSQuery.ResetQuery();
    }

    public override NodeResult Execute()
    {
        if (EQSQuery == null) return NodeResult.failure;

        EQSQuery.Progress();

        if (EQSQuery != null && EQSQuery.QueryStatus == EQSStatus.Finished && EQSQuery.BestResult != null)
        {
            AimLocationReference.Value = EQSQuery.BestResult.GetWorldPosition();
            return NodeResult.success;
        }

        return NodeResult.running;
    }
}
