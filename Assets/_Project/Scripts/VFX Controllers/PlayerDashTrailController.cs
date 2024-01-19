using UnityEngine;

public class PlayerDashTrailController : MonoBehaviour
{
    [SerializeField] 
    private GameObject playerDashTrailGO;
    
    private TrailRenderer playerDashTrail;
    private float trailTime;
    private float delayTime = 0.04f;
    
    private void Start()
    {
        playerDashTrail = playerDashTrailGO.GetComponent<TrailRenderer>();
        trailTime = playerDashTrail.time;
        playerDashTrail.time = 0f;
        
        GetComponent<PlayerMovement>().OnDashPerformed += OnDashPerformed;
    }

    private void FixedUpdate()
    {
        if (playerDashTrail.time > 0f)
        {
            playerDashTrail.time -= delayTime;
        }
    }

    private void OnDashPerformed()
    {
        playerDashTrail.time = trailTime;
    }
}
