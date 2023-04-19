using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBaseState
{
    protected NavMeshAgent navMeshAgent;
    protected GameObject playerRef;
    protected GameObject enemyRef;
    protected GameObject weaponRef;

    public EnemyBaseState(NavMeshAgent navMeshAgent, GameObject playerRef, GameObject enemyRef, GameObject weaponRef)
    {
        this.navMeshAgent = navMeshAgent;
        this.playerRef = playerRef;
        this.enemyRef = enemyRef;
        this.weaponRef = weaponRef;
    }
    
    public virtual void ChangePlayerRef(GameObject playerRef)
    {
        this.playerRef = playerRef;
    }

    public abstract void EnterState(EnemyAIManager enemy);
    public abstract void UpdateState(EnemyAIManager enemy);
}
