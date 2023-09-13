using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    private static ReferenceManager instance;
    private BlakeHeroCharacter blakeHeroCharacter;
    private PlayerInputController playerInputController;
    private SceneHandler sceneHandler;
    private MessageRouter messageRouter = new();

    public static BlakeHeroCharacter BlakeHeroCharacter
    {
        get => instance != null ? instance.blakeHeroCharacter : null;
        set
        {
            if (instance == null) return;
            instance.blakeHeroCharacter = value;
        }
    }
    
    public static PlayerInputController PlayerInputController
    {
        get => instance != null ? instance.playerInputController : null;
        set
        {
            if (instance == null) return;
            instance.playerInputController = value;
        }
    }
    
    public static SceneHandler SceneHandler
    {
        get => instance != null ? instance.sceneHandler : null;
        set
        {
            if (instance == null) return;
            instance.sceneHandler = value;
        }
    }

    public static MessageRouter MessageRouter => instance != null ? instance.messageRouter : null;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }
}
