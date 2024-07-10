using System.Threading.Tasks;
using _Project.Scripts.Patterns;
using _Project.Scripts.Player;
using _Project.Scripts.SceneHandler;
using _Project.Scripts.VirtualCamera;
using Unity.Services.Core;

namespace _Project.Scripts.GlobalHandlers
{
    public class ReferenceManager : Singleton<ReferenceManager>
    {
        private BlakeHeroCharacter blakeHeroCharacter;
        private PlayerInputController playerInputController;
        private SceneHandler.SceneHandler sceneHandler;
        private LevelHandler levelHandler;
        private MessageRouter messageRouter = new();
        private RoomManager roomManager;
        private MainVirtualCameraController mainVirtualCameraController;

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
    
        public static SceneHandler.SceneHandler SceneHandler
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
        
        public static MainVirtualCameraController MainVirtualCameraController
        {
            get => Instance != null ? Instance.mainVirtualCameraController : null;
            set
            {
                if (Instance == null) return;
                Instance.mainVirtualCameraController = value;
            }
        }

        protected override void Awake()
        {
            base.Awake();
#if ENABLE_CLOUD_SERVICES_ANALYTICS
            Task initializationTask = UnityServices.InitializeAsync();
#endif
        }
    }
}
