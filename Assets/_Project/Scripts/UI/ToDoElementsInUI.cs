using UnityEngine;
using UnityEngine.UI;

public class ToDoElementsInUI : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void Start()
    {
        _button.onClick.AddListener(() => ButtonClicked());
    }

    private void ButtonClicked()
    {
        Debug.LogError(this + "doesn't have UI implementation YET!");
    }
}
