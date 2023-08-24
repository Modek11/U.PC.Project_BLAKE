using UnityEngine;
using UnityEngine.UI;

public class LoadingSceneHandler : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    private void Update()
    {
        progressBar.fillAmount = ReferenceManager.SceneHandler.loadingProgress;
    }
}
