using UnityEngine;
using UnityEngine.UI;

public class ButtonNavigationHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject gameObjectToClose;

    [SerializeField]
    private GameObject gameObjectToOpen;
    
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void Start()
    {
        _button.onClick.AddListener(() => ButtonClicked());
    }

    public void ButtonClicked()
    {
        gameObjectToClose.SetActive(false);
        gameObjectToOpen.SetActive(true);
    }
}
