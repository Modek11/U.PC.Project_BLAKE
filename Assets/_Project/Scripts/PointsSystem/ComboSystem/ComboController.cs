using System;
using UnityEngine;

public class ComboController : MonoBehaviour
{
    private const float MAX_TIMER_VALUE = 3f;
    private const float MAX_COMBO_COUNT = 2f;
    private const float MIN_COMBO_COUNT = 1f;
    private const float COMBO_INCREASE_STEP = .1f;
    
    private float comboCounter = 1f;
    private float timer = 0f;
    private int killsCounter = 0;
    private bool isComboActive = false;

    public event Action OnComboTimerEnd;

    public float ComboCounter => comboCounter;
    public int KillsCounter => killsCounter;

    private void Update()
    {
        CountdownTime();
    }

    public void RegisterEnemyDeath()
    {
        timer = MAX_TIMER_VALUE;
        killsCounter++;

        isComboActive = true;

        if (comboCounter < MAX_COMBO_COUNT)
        {
            comboCounter += COMBO_INCREASE_STEP;
        }
    }
    
    private void CountdownTime()
    {
        if (!isComboActive)
        {
            return;
        }
        
        timer -= Time.deltaTime;
        
        if (timer < 0)
        {
            ResetValues();
        }
    }

    private void ResetValues()
    {
        OnComboTimerEnd?.Invoke();
        timer = 0f;
        killsCounter = 0;
        comboCounter = MIN_COMBO_COUNT;
        isComboActive = false;
    }
}
