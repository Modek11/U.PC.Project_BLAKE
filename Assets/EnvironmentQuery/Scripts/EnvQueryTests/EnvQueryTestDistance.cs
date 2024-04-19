using System.Collections.Generic;
using _Project.Scripts;
using UnityEngine;

[CreateAssetMenu(menuName = "EQS/Distance")]
public class EnvQueryTestDistance : EnvQueryTest
{
    public Transform DistanceTo;
    public float MinDistance;
    public float MaxDistance;

    public override void RunTest(int currentTest, List<EnvQueryItem> envQueryItems)
    {
        if(DistanceTo == null) DistanceTo = ReferenceManager.BlakeHeroCharacter?.transform;

        if (IsActive && DistanceTo != null && envQueryItems != null)
        {
            foreach(EnvQueryItem item in envQueryItems)
            {
                float distance = Vector3.Distance(DistanceTo.position, item.GetWorldPosition());
                
                if (distance > MaxDistance || distance < MinDistance)
                { 
                    item.TestResults[currentTest] = 0.0f;
                    continue; 
                }

                item.TestResults[currentTest] = distance;
            }
        }
        else
        {
            foreach(EnvQueryItem item in envQueryItems)
            {
                item.TestResults[currentTest] = 0.0f;
            }
        }
    }
}