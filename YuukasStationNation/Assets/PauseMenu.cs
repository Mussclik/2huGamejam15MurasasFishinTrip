using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settings;

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider generalVolume;

    [SerializeField] private Toggle isAnimationsDisabled;
    private void Start()
    {
        resumeButton.onClick.AddListener(GameManager.instance.MenuCheck);
        settingsButton.onClick.AddListener(ToggleSettings);
        backButton.onClick.AddListener(ToggleSettings);
        quitButton.onClick.AddListener(Application.Quit);

        masterVolume.onValueChanged.AddListener((float value) => SoundManager.instance.ChangeMasterVolume(value));
        musicVolume.onValueChanged.AddListener((float value) => SoundManager.instance.ChangeMusicVolume(value));
        generalVolume.onValueChanged.AddListener((float value) => SoundManager.instance.ChangeGeneralVolume(value));

        isAnimationsDisabled.onValueChanged.AddListener((bool value) => GameManager.isAnimationsDisabled = value);

        SoundManager.instance.ChangeMasterVolume(masterVolume.value);
        SoundManager.instance.ChangeMusicVolume(musicVolume.value);
        SoundManager.instance.ChangeGeneralVolume(generalVolume.value);

        #if UNITY_WEBGL
        quitButton.gameObject.SetActive(false);
        #endif
    }
    private void OnEnable()
    {
        mainMenu.SetActive(true);
        settings.SetActive(false);
    }

    public void ToggleSettings()
    {
        if (mainMenu.activeSelf)
        {
            mainMenu.SetActive(false);
            settings.SetActive(true);
        }
        else
        {
            mainMenu.SetActive(true);
            settings.SetActive(false);
        }
    }
}
