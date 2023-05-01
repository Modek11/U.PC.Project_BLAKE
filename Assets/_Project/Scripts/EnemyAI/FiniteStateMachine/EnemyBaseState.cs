using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBaseState
{
    protected NavMeshAgent navMeshAgent;
    protected EnemyAIManager aiManager;

    public EnemyBaseState(NavMeshAgent navMeshAgent, EnemyAIManager aIManager)
    {
        this.navMeshAgent = navMeshAgent;
        this.aiManager = aIManager;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
}
