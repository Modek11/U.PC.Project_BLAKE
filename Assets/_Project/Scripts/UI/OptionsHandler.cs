using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class OptionsHandler : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown resolutionsDropdown;

    [SerializeField]
    private TextMeshProUGUI fullScreenValueText;

    [SerializeField]
    private TextMeshProUGUI masterVolumeValueText;

    [SerializeField]
    private TextMeshProUGUI musicVolumeValueText;

    [SerializeField]
    private TextMeshProUGUI sfxVolumeValueText;

    public void Start()
    {
        SetResolutions();
    }

    private void SetResolutions()
    {
        if (resolutionsDropdown == null)
        {
            Debug.LogWarning("Dropdown is empty!");
            return;
        }
        
        resolutionsDropdown.options.Clear(); 
        foreach (var resolution in Screen.resolutions)
        {
            resolutionsDropdown.options.Add(new TMP_Dropdown.OptionData($"{resolution.width} x {resolution.height}"));
        }
        resolutionsDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        var resolution = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,resolution.height,Screen.fullScreen);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        fullScreenValueText.text = isFullScreen ? "ON" : "OFF";
        Screen.fullScreen = isFullScreen;
    }

    public void SetMasterVolume(float volume)
    {
        masterVolumeValueText.text = volume + 80 + "%";
        Debug.LogWarning("SetMasterVolume not implemented!");
        //connect with AudioMixer
    }

    public void SetMusicVolume(float volume)
    {
        musicVolumeValueText.text = volume + 80 + "%";
        Debug.LogWarning("SetMusicVolume not implemented!");
        //connect with AudioMixer
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolumeValueText.text = volume + 80 + "%";
        Debug.LogWarning("SetSfxVolume not implemented!");
        //connect with AudioMixer
    }
}
