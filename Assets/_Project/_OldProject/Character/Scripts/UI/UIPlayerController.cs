using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rifleName;
    [SerializeField] private TextMeshProUGUI magazineLeft;
    [SerializeField] private GameObject reloadingGameObject;
    [SerializeField] private Image reloadingImage;
    [SerializeField] private TextMeshProUGUI noAmmoLeftGameObject;
    
    
    private void Awake()
    {
        if(reloadingImage != null)
        reloadingGameObject.SetActive(false);
    }

    public void GunNameUI(string nameToDisplay)
    {
        rifleName.text = nameToDisplay;
    }
    
    public void MagazineSizeUI(byte bulletsLeft,byte magazineSize)
    {
        magazineLeft.text = $"{bulletsLeft.ToString()} / {magazineSize.ToString()}";

        noAmmoLeftGameObject.alpha = bulletsLeft <= 0 ? 255 : 0;
    }

    public void ReloadUI(bool isReloading, float reloadTime)
    {
        switch (isReloading)
        {
            case true:
                reloadingGameObject.SetActive(true);
                reloadingImage.fillAmount += 1.0f / reloadTime * Time.deltaTime;
                break;
            case false:
                reloadingGameObject.SetActive(false);
                reloadingImage.fillAmount = 0;
                break;
        }
    }
    public void SetUI(PlayerUIObject ui)
    {
        rifleName = ui.GetRifleName();
        magazineLeft = ui.GetMagazineLeft();
        reloadingGameObject = ui.GetReloadingObject();
        reloadingGameObject.SetActive(false);
        reloadingImage = ui.GetReloadingImage();
        noAmmoLeftGameObject = ui.GetNoAmmoLeft();
    }
    
}
