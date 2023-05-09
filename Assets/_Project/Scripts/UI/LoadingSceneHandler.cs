using UnityEngine;
using UnityEngine.UI;

public class LoadingSceneHandler : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    private void Update()
    {
        progressBar.fillAmount = SceneHandler.Instance.loadingProgress;
    }
}
