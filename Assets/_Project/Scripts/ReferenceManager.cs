using UnityEngine;
using Unity.Services.Analytics;
using System.Threading.Tasks;



#if ENABLE_CLOUD_SERVICES_ANALYTICS
using Unity.Services.Core;
#endif

public class ReferenceManager : MonoBehaviour
{
    private static ReferenceManager instance;
    private BlakeHeroCharacter blakeHeroCharacter;
    private PlayerInputController playerInputController;
    private SceneHandler sceneHandler;
    private MessageRouter messageRouter = new();
    private RoomManager roomManager;

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

    public static MessageRouter MessageRouter
    {
        get => instance != null ? instance.messageRouter : null;
    }

    public static RoomManager RoomManager
    {
        get => instance != null ? instance.roomManager : null;
        set
        {
            if (instance == null) return;
            instance.roomManager = value;
        }
    }

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

#if ENABLE_CLOUD_SERVICES_ANALYTICS
            Task initializationTask = UnityServices.InitializeAsync();
#endif
        }
    }
}
