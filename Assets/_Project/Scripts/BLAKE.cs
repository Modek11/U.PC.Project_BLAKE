using UnityEngine;

public class BLAKE : MonoBehaviour
{
    private static BLAKE Instance;

    private BlakeHeroCharacter _hero;
    public static BlakeHeroCharacter Hero
    {
        get => Instance != null ? Instance._hero : null;
        set
        {
            if (Instance == null) return;
            Instance._hero = value;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
