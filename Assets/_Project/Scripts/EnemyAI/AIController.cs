using MBT;
using UnityEngine;
using UnityEngine.AI;
using static EnemyFOV;

[RequireComponent(typeof(BlakeCharacter))]
public class AIController : MonoBehaviour
{
    public Weapon Weapon;

    [HideInInspector]
    public NavMeshAgent NavMeshAgent;

    private GameObject playerRef;
    private Animator animator;

    public CombatStateReference CombatStateReference;
    public BoolReference HasLineOfSightReference;

    public Waypoints Waypoints;

    private void Awake()
    {
        UpdatePlayerRef();
        Weapon.Owner = gameObject;

        NavMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        GetComponent<BlakeCharacter>().onDeath += Die;
        GetComponent<EnemyFOV>().OnCanSeePlayerChanged += OnCanSeePlayerChanged;
    }

    private void Die()
    {
        this.enabled = false;
        NavMeshAgent.isStopped = true;
        GetComponent<MBTExecutor>().enabled = false;
    }

    private void Update()
    {
        FaceThePlayer();

        animator.SetFloat("Direction", BlakeAnimatorHelper.CalculateDirection(NavMeshAgent.velocity, transform));
        animator.SetFloat("Speed", NavMeshAgent.velocity.magnitude);
    }

    public void UpdatePlayerRef()
    {
        playerRef = ReferenceManager.BlakeHeroCharacter?.gameObject;
    }

    public void SetWaypoints(Waypoints waypoints)
    {
        this.Waypoints = waypoints;
    }

    private void FaceThePlayer()
    {
        if (playerRef == null)
        {
            return;
        }

        if (CombatStateReference.GetVariable().Value != CombatState.Patrol)
        {
            Vector3 direction = (playerRef.transform.position - gameObject.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, Time.deltaTime * 40f);
        }
    }

    private void OnCanSeePlayerChanged(bool newCanSeePlayer)
    {
        HasLineOfSightReference.Value = newCanSeePlayer;
        if (newCanSeePlayer)
        {
            CombatStateReference.GetVariable().Value = CombatState.Attack;
            if (IsInvoking("ClearPlayerFocus"))
            {
                CancelInvoke("ClearPlayerFocus");
            }
        }
        else
        {
            Invoke("ClearPlayerFocus", 10f);
        }
    }

    private void ClearPlayerFocus()
    {
        CombatStateReference.GetVariable().Value = CombatState.Patrol;
    }
}
