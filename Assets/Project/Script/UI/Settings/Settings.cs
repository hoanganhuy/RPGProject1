using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle fullscreenToggle;

    private const string VOLUME_KEY = "MasterVolume";
    private const string FULLSCREEN_KEY = "Fullscreen";

    private void Start()
    {
        // Load saved settings
        float savedVolume = PlayerPrefs.GetFloat(VOLUME_KEY, 0.5f);
        bool savedFullscreen = PlayerPrefs.GetInt(FULLSCREEN_KEY, 1) == 1;

        volumeSlider.value = savedVolume;
        fullscreenToggle.isOn = savedFullscreen;

        // Apply initially
        AudioListener.volume = savedVolume;
        Screen.fullScreen = savedFullscreen;

        // Add listeners
        volumeSlider.onValueChanged.AddListener(SetVolume);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat(VOLUME_KEY, value);
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt(FULLSCREEN_KEY, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }
}