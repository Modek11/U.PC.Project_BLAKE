using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIObject : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rifleName;
    [SerializeField] private TextMeshProUGUI magazineLeft;
    [SerializeField] private GameObject reloadingGameObject;
    [SerializeField] private Image reloadingImage;
    [SerializeField] private TextMeshProUGUI noAmmoLeftGameObject;

    public TextMeshProUGUI GetRifleName()
    {
        return rifleName;
    }

    public TextMeshProUGUI GetMagazineLeft()
    {
        return magazineLeft;
    }

    public GameObject GetReloadingObject()
    {
        return reloadingGameObject;
    }

    public Image GetReloadingImage()
    {
        return reloadingImage;
    }

    public TextMeshProUGUI GetNoAmmoLeft()
    {
        return noAmmoLeftGameObject;
    }
}
