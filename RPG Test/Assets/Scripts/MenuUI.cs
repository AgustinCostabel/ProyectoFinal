using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{

    public static MenuUI Instance { get; private set; }

    private const string PLAYER_PREFS_QUALITY = "Quality";
    private const string PLAYER_PREFS_FULLSCREEN = "FullScreen";

    [SerializeField] private GameObject menuActivation;
    [SerializeField] private GameObject spellsUI;
    [SerializeField] private Slider volumeSliderMusic;
    [SerializeField] private Slider volumeSliderSFX;
    [SerializeField] private Slider volumeSliderMaster;
    [SerializeField] private TMP_Dropdown graphicsDropdown;
    [SerializeField] private TMP_Dropdown resolutionsDropdown;
    [SerializeField] private Toggle fullScreenToggle;

    Resolution[] resolutions;

    private int qualityIndex = 0;
    private int isFullScreenInt;
    private bool isFullScreen;
    private bool timelineActive;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("ERROR: MORE THAN ONE MENU UI");
        }
        Instance = this;

        qualityIndex = PlayerPrefs.GetInt(PLAYER_PREFS_QUALITY, qualityIndex);
        QualitySettings.SetQualityLevel(qualityIndex);

        isFullScreenInt = PlayerPrefs.GetInt(PLAYER_PREFS_FULLSCREEN, isFullScreenInt);
        isFullScreen = (isFullScreenInt == 1);
        Screen.fullScreen = isFullScreen;
        fullScreenToggle.isOn = isFullScreen;
    }

    private void Start() {
        GameInput.Instance.OnMenuAction += GameInput_OnMenuAction;

        graphicsDropdown.value = qualityIndex;

        //Resolutions
        resolutions = Screen.resolutions;
        resolutionsDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height) {
                currentResolutionIndex = i;
            }
        }

        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = currentResolutionIndex;
        resolutionsDropdown.RefreshShownValue();
    }

    private void GameInput_OnMenuAction(object sender, System.EventArgs e) {
        if (!timelineActive) {
            if (!menuActivation.gameObject.activeSelf) {
                Show();
                GameManager.Instance.PauseGame();
            } else {
                Hide();
                GameManager.Instance.UnpauseGame();
            }
        }
    }

    private void Hide() {
        menuActivation.gameObject.SetActive(false);
    }

    private void Show() {
        menuActivation.gameObject.SetActive(true);
    }

    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);

        PlayerPrefs.SetInt(PLAYER_PREFS_QUALITY, qualityIndex);
        PlayerPrefs.Save();
    }

    public void SetFullScreen(bool isFullScreen) {
        Screen.fullScreen = isFullScreen;

        PlayerPrefs.SetInt("PLAYER_PREFS_FULLSCREEN", (isFullScreen ? 1 : 0));
        PlayerPrefs.Save();
    }

    public void SetResoltion(int resolutionIndex) {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetSlidersSound(float master, float music, float sfx) {
        volumeSliderMaster.value = master;
        volumeSliderMusic.value = music;
        volumeSliderSFX.value = sfx;
    }

    public int GetQuality() {
        return qualityIndex;
    }

    public void SetTimelineActive(bool active) {
        timelineActive = active;
    }

}
