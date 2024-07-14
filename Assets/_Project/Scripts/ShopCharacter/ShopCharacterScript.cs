using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopCharacterScript : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject shopMenu;
    [SerializeField]
    private Transform uiPos;
    private bool isOpen;
    public bool CanInteract()
    {
        return true;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public Vector3 GetPositionForUI()
    {
        return uiPos.position;
    }

    public void Interact(GameObject interacter)
    {
        if(!isOpen)
        {
            OpenShop();
        }
    }

    private void OpenShop()
    {
        shopMenu.SetActive(true);
        Time.timeScale = 0f;
        isOpen = true;
    }

    public void CloseShop()
    {
        shopMenu.SetActive(false);
        Time.timeScale = 1f;
        isOpen = false;
    }


}
