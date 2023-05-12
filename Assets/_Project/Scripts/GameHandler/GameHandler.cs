using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance { get; private set; }

    [SerializeField] private GameObject pausedGameCanvas;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
            
            //Ustawić dostęp poprzez skrypt aby nie wypinało referencji
            //DontDestroyOnLoad(Instance);
        }
    }

    public void PlayerWin()
    {
        Time.timeScale = 0f;
        for(int i = 0; i < pausedGameCanvas.transform.childCount; i++)
        {
            var child = pausedGameCanvas.transform.GetChild(i).gameObject;
            child.SetActive(child.name is "BackgroundFade" or "YouWin_Canvas");
        }

        pausedGameCanvas.SetActive(true);
    }

    public void PlayerLose()
    {
        Time.timeScale = 0f;
        for(int i = 0; i < pausedGameCanvas.transform.childCount; i++)
        {
            var child = pausedGameCanvas.transform.GetChild(i).gameObject;
            child.SetActive(child.name is "BackgroundFade" or "YouLose_Canvas");
        }
        
        pausedGameCanvas.SetActive(true);
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
