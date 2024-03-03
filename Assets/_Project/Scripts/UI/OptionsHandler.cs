using System;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;

public class OptionsHandler : MonoBehaviour
{
    private const string MASTER = "Master";
    private const string MUSIC = "Music";
    private const string SFX = "SFX";
    private const string ENVIRONMENT_SFX = "EnvironmentSFX";
    private const string PLAYER_SFX = "PlayerSFX";
    private const string ENEMY_SFX = "EnemySFX";
    
    private const string ON = "ON";
    private const string OFF = "OFF";
    private const string PERCENTAGE_SYMBOL = "%";

    [SerializeField] 
    private AudioMixer mainAudioMixer;
    
    [Space]
    
    [SerializeField]
    private TMP_Dropdown resolutionsDropdown;

    [SerializeField]
    private TextMeshProUGUI fullScreenValueText;

    [SerializeField]
    private TextMeshProUGUI vSyncValueText;

    [SerializeField]
    private TMP_Dropdown qualityDropdown;

    [Space]
    
    [SerializeField]
    private TextMeshProUGUI masterVolumeValueText;

    [SerializeField] 
    private Slider masterVolumeSlider;

    [SerializeField]
    private TextMeshProUGUI musicVolumeValueText;

    [SerializeField] 
    private Slider musicVolumeSlider;

    [SerializeField]
    private TextMeshProUGUI sfxVolumeValueText;
    [SerializeField] 
    private Slider sfxVolumeSlider;
    
    [SerializeField]
    private TextMeshProUGUI environmentSfxVolumeValueText;

    [SerializeField] 
    private Slider environmentSfxVolumeSlider;

    [SerializeField]
    private TextMeshProUGUI playerSfxVolumeValueText;

    [SerializeField] 
    private Slider playerSfxVolumeSlider;

    [SerializeField]
    private TextMeshProUGUI enemySfxVolumeValueText;

    [SerializeField] 
    private Slider enemySfxVolumeSlider;

    public void Start()
    {
        SetResolutions();
        SetAllSliders();
        SetGraphics();
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
            //bool alreadyExist = false;
            //for(int i = 0; i < resolutionsDropdown.options.Count; i++)
            //{
            //    if (resolutionsDropdown.options[i].text == $"{resolution.width} x {resolution.height}")
            //    {
            //        alreadyExist = true;
            //        break;
            //    }
            //}
            //if (!alreadyExist)
            //{
                resolutionsDropdown.options.Add(new TMP_Dropdown.OptionData($"{resolution.width} x {resolution.height}"));
            //}
        }

        for (int i = 0; i < resolutionsDropdown.options.Count; i++)
        {
            if (resolutionsDropdown.options[i].text == $"{Screen.currentResolution.width} x {Screen.currentResolution.height}")
            {
                resolutionsDropdown.value = i;
                break;
            }
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        var resolution = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,resolution.height,Screen.fullScreen);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        fullScreenValueText.text = isFullScreen ? ON : OFF;
        Screen.fullScreen = isFullScreen;
    }

    public void SetVSync(bool enabled)
    {
        vSyncValueText.text = enabled ? ON : OFF;
        QualitySettings.vSyncCount = enabled ? 1 : 0;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    private void SetAllSliders()
    {
        masterVolumeSlider.value = GetVolume(MASTER);
        musicVolumeSlider.value = GetVolume(MUSIC);
        sfxVolumeSlider.value = GetVolume(SFX);
        environmentSfxVolumeSlider.value = GetVolume(ENVIRONMENT_SFX);
        playerSfxVolumeSlider.value = GetVolume(PLAYER_SFX);
        enemySfxVolumeSlider.value = GetVolume(ENEMY_SFX);
    }

    private void SetGraphics()
    {
        fullScreenValueText.text = Screen.fullScreen ? ON : OFF;
        fullScreenValueText.transform.parent.GetComponent<Toggle>().isOn = Screen.fullScreen ? true : false;

        vSyncValueText.text = QualitySettings.vSyncCount > 0 ? ON : OFF;
        vSyncValueText.transform.parent.GetComponent<Toggle>().isOn = QualitySettings.vSyncCount > 0 ? true : false;

        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new List<TMP_Dropdown.OptionData>()
                { new TMP_Dropdown.OptionData("Low")
                , new TMP_Dropdown.OptionData("Medium")
                , new TMP_Dropdown.OptionData("High")
                , new TMP_Dropdown.OptionData("Ultra") });

        qualityDropdown.value = QualitySettings.GetQualityLevel();
    }

    public void SetMasterVolume(float volume)
    {
        SetVolumeAndText(volume, MASTER, masterVolumeValueText);
    }

    public void SetMusicVolume(float volume)
    {
        SetVolumeAndText(volume, MUSIC, musicVolumeValueText);
    }

    public void SetSfxVolume(float volume)
    {
        SetVolumeAndText(volume, SFX, sfxVolumeValueText);
    }
    
    public void SetEnvironmentSfxVolume(float volume)
    {
        SetVolumeAndText(volume, ENVIRONMENT_SFX, environmentSfxVolumeValueText);
    }
    
    public void SetPlayerSfxVolume(float volume)
    {
        SetVolumeAndText(volume, PLAYER_SFX, playerSfxVolumeValueText);
    }
    
    public void SetEnemySfxVolume(float volume)
    {
        SetVolumeAndText(volume, ENEMY_SFX, enemySfxVolumeValueText);
    }

    private void SetVolumeAndText(float volume, string mixerName, TextMeshProUGUI textMeshProRef)
    {
        textMeshProRef.text = GetVolumeString(volume);
        mainAudioMixer.SetFloat(mixerName, MathF.Log10(volume) * 20);
    }

    private float GetVolume(string mixerName)
    {
        mainAudioMixer.GetFloat(mixerName, out float volume);
        return MathF.Pow(10, volume / 20);
    }
    
    private string GetVolumeString(float volume)
    {
        volume *= 100;
        return $"{volume.ToString("N0")}{PERCENTAGE_SYMBOL}";
    }
}
