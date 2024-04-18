using System;
using _Project.Scripts;
using Cinemachine;
using UnityEngine;

public class ShakeCameraOnShoot : MonoBehaviour
{
    [SerializeField] 
    private float amplitude;
    [SerializeField] 
    private float frequency;
    [SerializeField] 
    private float delayTime;
    
    private CinemachineBasicMultiChannelPerlin channelPerlin;
    
    void Start()
    {
        channelPerlin = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        ReferenceManager.BlakeHeroCharacter.GetComponent<WeaponsManager>().OnPrimaryAttack += ShakeOnShoot;
    }

    private void FixedUpdate()
    {
        if (channelPerlin.m_AmplitudeGain > 0f)
        {
            channelPerlin.m_AmplitudeGain -= delayTime;
        }
    }

    private void ShakeOnShoot(Weapon obj)
    {
        channelPerlin.m_AmplitudeGain = amplitude;
        channelPerlin.m_FrequencyGain = frequency;
    }
}
