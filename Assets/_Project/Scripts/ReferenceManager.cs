using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    private static ReferenceManager Instance;

    private BlakeHeroCharacter blakeHeroCharacter;
    private PlayerInputController playerInputController;
    private SceneHandler sceneHandler;
    public static BlakeHeroCharacter BlakeHeroCharacter
    {
        get => Instance != null ? Instance.blakeHeroCharacter : null;
        set
        {
            if (Instance == null) return;
            Instance.blakeHeroCharacter = value;
        }
    }
    
    public static PlayerInputController PlayerInputController
    {
        get => Instance != null ? Instance.playerInputController : null;
        set
        {
            if (Instance == null) return;
            Instance.playerInputController = value;
        }
    }
    
    public static SceneHandler SceneHandler
    {
        get => Instance != null ? Instance.sceneHandler : null;
        set
        {
            if (Instance == null) return;
            Instance.sceneHandler = value;
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
            DontDestroyOnLoad(Instance);
        }
    }
}
