using MBT;
using UnityEditor.Search;
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

        if(!EQSQuery.enabled)
        {
            EQSQuery.enabled = true;
        }

        if (EQSQuery != null && EQSQuery.BestResult != null)
        {
            AimLocationReference.Value = EQSQuery.BestResult.GetWorldPosition();
            EQSQuery.enabled = false;
            return NodeResult.success;
        }

        return NodeResult.running;
    }
}
