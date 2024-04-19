using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.VFX_Controllers
{
    public class PlayerDashTrailController : MonoBehaviour
    {
        [SerializeField] 
        private TrailRenderer playerDashTrail;
    
        private float trailTime;
        private float delayTime = 0.04f;
    
        private void Start()
        {
            trailTime = playerDashTrail.time;
            playerDashTrail.time = 0f;
            playerDashTrail.gameObject.SetActive(false);
        
            GetComponent<PlayerMovement>().OnDashPerformed += OnDashPerformed;
        }

        private void OnDashPerformed()
        {
            UniTask.Void(async () =>
            {
                playerDashTrail.gameObject.SetActive(true);
                playerDashTrail.time = trailTime;
            
                while (playerDashTrail.time > 0f)
                {
                    playerDashTrail.time -= delayTime;
                    await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
                }
            
                playerDashTrail.time = 0f;
                playerDashTrail.gameObject.SetActive(false);
            });
        }
    }
}
