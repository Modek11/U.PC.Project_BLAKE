using MBT;
using UnityEngine;
using UnityEngine.AI;
using static EnemyFOV;

[RequireComponent(typeof(BlakeCharacter))]
public class AIController : MonoBehaviour
{
    [SerializeField]
    private GameObject weaponRef;

    [HideInInspector]
    public NavMeshAgent navMeshAgent;

    private GameObject playerRef;
    private Animator animator;

    public CombatStateReference combatStateReference;
    public BoolReference hasLineOfSightReference;

    public Waypoints waypoints;

    private void Awake()
    {
        UpdatePlayerRef();

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        GetComponent<BlakeCharacter>().onDeath += Die;
        GetComponent<EnemyFOV>().OnCanSeePlayerChanged += OnCanSeePlayerChanged;
    }

    private void Die()
    {
        this.enabled = false;
        navMeshAgent.isStopped = true;
        GetComponent<MBTExecutor>().enabled = false;
    }

    private void Update()
    {
        FaceThePlayer();

        animator.SetFloat("Direction", BlakeAnimatorHelper.CalculateDirection(navMeshAgent.velocity, transform));
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    }

    public void UpdatePlayerRef()
    {
        playerRef = ReferenceManager.BlakeHeroCharacter?.gameObject;
    }

    public void SetWaypoints(Waypoints waypoints)
    {
        this.waypoints = waypoints;
    }

    private void FaceThePlayer()
    {
        if (playerRef == null)
        {
            return;
        }

        if (combatStateReference.GetVariable().Value != CombatState.Patrol)
        {
            Vector3 direction = (playerRef.transform.position - gameObject.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, Time.deltaTime * 40f);
        }
    }

    private void OnCanSeePlayerChanged(bool newCanSeePlayer)
    {
        hasLineOfSightReference.Value = newCanSeePlayer;
        if (newCanSeePlayer)
        {
            combatStateReference.GetVariable().Value = CombatState.Attack;
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
        combatStateReference.GetVariable().Value = CombatState.Patrol;
    }

    public GameObject GetWeaponRef()
    {
        return weaponRef;
    }
}
