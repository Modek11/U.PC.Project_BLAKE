using UnityEngine;

public class ContinueButtonVisibility : MonoBehaviour
{
    [SerializeField] private GameObject blankSpacing;
    [SerializeField] private GameObject continueButton;
    
    private bool _playerHasUnfinishedGame = false;

    public void Start()
    {
        blankSpacing.SetActive(!_playerHasUnfinishedGame);
        continueButton.SetActive(_playerHasUnfinishedGame);
    }
}
