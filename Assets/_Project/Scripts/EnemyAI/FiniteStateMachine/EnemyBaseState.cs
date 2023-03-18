using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBaseState
{
    protected NavMeshAgent navMeshAgent;
    protected GameObject playerRef;
    protected GameObject enemyRef;

    public EnemyBaseState(NavMeshAgent navMeshAgent, GameObject playerRef, GameObject enemyRef)
    {
        this.navMeshAgent = navMeshAgent;
        this.playerRef = playerRef;
        this.enemyRef = enemyRef;
    }

    public abstract void EnterState(EnemyAIManager enemy);
    public abstract void UpdateState(EnemyAIManager enemy);
}
