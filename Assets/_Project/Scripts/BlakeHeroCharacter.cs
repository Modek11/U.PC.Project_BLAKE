using System.Collections.Generic;
using _Project.Scripts;
using _Project.Scripts.GlobalHandlers;
using _Project.Scripts.PointsSystem;
using Unity.Mathematics;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
using Unity.Services.Analytics;
using UnityEngine;
#endif

public class BlakeHeroCharacter : BlakeCharacter
{

    private void Awake()
    {
        ReferenceManager.BlakeHeroCharacter = this;

        defaultHealth = ReferenceManager.SceneHandler.isNormalDifficulty ? 3 : 1;
        health = defaultHealth;
        respawnCounter = 0;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        onDeath += EnemyDeathMediator.Instance.PlayerCurrencyController.LosePointsOnDeath;
    }

    private void OnDestroy()
    {
        ReferenceManager.BlakeHeroCharacter = null;
    }

    public override void Die(GameObject killer)
    {
#if ENABLE_CLOUD_SERVICES_ANALYTICS
        if (killer != null)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "killer", killer.name },
            { "itemName", killer.GetComponent<AIController>()?.Weapon?.name },
            { "placementName", ReferenceManager.RoomManager.GetActiveRoom().name }
        };

            AnalyticsService.Instance.StartDataCollection();
            AnalyticsService.Instance.CustomData("HeroDead", parameters);
            Debug.Log("Analytics data sent.");
        }
#endif

        explosionParticleInstantiated = Instantiate(explosionParticle, transform.position, quaternion.identity);
        gameObject.SetActive(false);
        Invoke("Respawn", 2f);

        base.Die(killer);
    }
}
