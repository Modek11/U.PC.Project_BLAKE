using System.Threading.Tasks;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
using Unity.Services.Core;
#endif

public class ReferenceManager : Singleton<ReferenceManager>
{
    private BlakeHeroCharacter blakeHeroCharacter;
    private PlayerInputController playerInputController;
    private SceneHandler sceneHandler;
    private LevelHandler levelHandler;
    private MessageRouter messageRouter = new();
    private RoomManager roomManager;

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

    public static LevelHandler LevelHandler
    {
        get => Instance != null ? Instance.levelHandler : null;
        set
        {
            if (Instance == null) return;
            Instance.levelHandler = value;
        }
    }

    public static MessageRouter MessageRouter
    {
        get => Instance != null ? Instance.messageRouter : null;
    }

    public static RoomManager RoomManager
    {
        get => Instance != null ? Instance.roomManager : null;
        set
        {
            if (Instance == null) return;
            Instance.roomManager = value;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
#if ENABLE_CLOUD_SERVICES_ANALYTICS
            Task initializationTask = UnityServices.InitializeAsync();
#endif
    }
}
